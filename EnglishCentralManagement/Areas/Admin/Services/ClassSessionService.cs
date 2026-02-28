using EnglishCentralManagement.Areas.Admin.Services.Interfaces;
using EnglishCentralManagement.Data;
using EnglishCentralManagement.Dtos;
using EnglishCentralManagement.Dtos.Pagination;
using EnglishCentralManagement.Extensions;
using EnglishCentralManagement.Helpers;
using EnglishCentralManagement.Models;
using EnglishCentralManagement.Models.Enum;
using Microsoft.EntityFrameworkCore;

namespace EnglishCentralManagement.Areas.Admin.Services
{
    public class ClassSessionService : IClassSessionService
    {
        private readonly EnglishCentreDbContext _context;
        private readonly ICurrentUserService _currentUser;
        public ClassSessionService(EnglishCentreDbContext context, ICurrentUserService currentUser)
        {
            _context = context;
            _currentUser = currentUser;
        }

        public async Task CreateSessionClass(CreateSessionDto createSession)
        {
            var model = new ClassSession
            {
                SessionName = createSession.SessionName,
                ClassId = (long)createSession.ClassId,
                SessionDate = createSession.SessionDate.ToUtcDb(),
                StartTime = createSession.StartTime,
                EndTime = createSession.EndTime,
                Status = createSession.Status,
                Note = createSession.Note,
                FeedBack = createSession.FeedBack,
                CreatedBy = _currentUser.FullName,
                CreatedDate = DateTimeOffset.UtcNow.ToUtcDb(),
            };
            _context.ClassSessions.Add(model);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteSession(long id)
        {
            var model = await _context.ClassSessions
                .Where(x => x.Id == id && !x.IsDeleted)
                .FirstOrDefaultAsync();
            if (model == null)
                throw new Exception("Session is not found");
            model.IsDeleted = true;
            await _context.SaveChangesAsync();
        }

        public async Task<PagedResult<SessionDto>> GetAllClassSession(long classId, int pageIndex, int pageSize, string search = null)
        {
            var query = _context.ClassSessions
                .Include(x => x.Attendances)
                .Where(x => x.ClassId == classId && !x.IsDeleted);
            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(x =>
                    x.SessionName.Contains(search));
            }
            var totalRecords = await query.CountAsync();
            var items = await query
                .OrderByDescending(x => x.Id)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .Select(x => new SessionDto
                {
                    Id = x.Id,
                    SessionName = x.SessionName,
                    Note = x.Note,
                    FeedBack = x.FeedBack,
                    Attendance =
                            x.Attendances.Count(a => a.Status == AttendanceStatusEnum.Present)
                            + "/" +
                            _context.Enrollments.Count(e => e.ClassId == x.ClassId),
                    StartTime = x.StartTime.ToString(),
                    EndTime = x.EndTime.ToString(),
                    SessionStatus = x.Status.GetDisplayEnumName(),
                    SessionDate = x.SessionDate.ToVnTime(),
                    CreatedBy = x.CreatedBy,
                    CreatedDate = x.CreatedDate.HasValue
                                    ? x.CreatedDate.Value.ToVnTime()
                                    : null,
                    UpdatedBy = x.UpdatedBy,
                    UpdatedDate = x.UpdatedDate.HasValue
                                    ? x.UpdatedDate.Value.ToVnTime()
                                    : null
                })
                .ToListAsync();
            return new PagedResult<SessionDto>
            {
                Items = items,
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalRecords = totalRecords
            };
        }

        public async Task<SessionDto> GetSessionDetail(long sessionId)
        {
            var model = await _context.ClassSessions
                .Where(x => x.Id == sessionId && !x.IsDeleted)
                .FirstOrDefaultAsync();
            if (model == null)
                throw new Exception("Session is not found");
            var data = new SessionDto
            {
                Id = model.Id,
                SessionName = model.SessionName,
                SessionDate = model.SessionDate,
                SessionStatusEnum = model.Status,
                StartTime = model.StartTime.ToString("HH:mm"),
                EndTime = model.EndTime.ToString("HH:mm"),
                Note = model.Note,
                FeedBack = model.FeedBack,
                CreatedBy = model.CreatedBy,
                CreatedDate = model.CreatedDate.HasValue
                                    ? model.CreatedDate.Value.ToVnTime()
                                    : null,
                UpdatedBy = model.UpdatedBy,
                UpdatedDate = model.UpdatedDate.HasValue
                                    ? model.UpdatedDate.Value.ToVnTime()
                                    : null
            };
            return data;
        }

        public async Task<List<StudentAttendance>> GetStudentsBySession(long id, long classId)
        {
            var data = await _context.Enrollments
                .Where(e => e.ClassId == classId
                            && e.Status == EnrollmentStatus.Active
                            && !e.IsDeleted)
                .Select(e => new StudentAttendance
                {
                    StudentId = e.Student.Id,
                    FullName = e.Student.FirstName + " " + e.Student.LastName,

                    AttendanceId = _context.Attendances
                        .Where(a => a.ClassSessionId == id
                                    && a.EnrollmentId == e.Id)
                        .Select(a => a.Id)
                        .FirstOrDefault(),

                    Status = _context.Attendances
                        .Where(a => a.ClassSessionId == id
                                    && a.EnrollmentId == e.Id)
                        .Select(a => (int?)a.Status)
                        .FirstOrDefault() ?? (int)AttendanceStatusEnum.Absent
                })
                .ToListAsync();

            return data;
        }

        public async Task SaveAttendance(long sessionId, List<StudentAttendanceRequest> model)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var classId = await _context.ClassSessions
                    .Where(s => s.Id == sessionId)
                    .Select(s => s.ClassId)
                    .FirstAsync();

                var enrollments = await _context.Enrollments
                    .Where(e => e.ClassId == classId)
                    .ToDictionaryAsync(e => e.StudentId, e => e.Id);

                var existingAttendances = await _context.Attendances
                    .Where(a => a.ClassSessionId == sessionId)
                    .ToListAsync();

                foreach (var item in model)
                {
                    if (!enrollments.TryGetValue((long)item.StudentId, out var enrollmentId))
                        continue;

                    var attendance = existingAttendances
                        .FirstOrDefault(a => a.EnrollmentId == enrollmentId);

                    if (attendance == null)
                    {
                        _context.Attendances.Add(new Attendance
                        {
                            ClassSessionId = sessionId,
                            EnrollmentId = enrollmentId,
                            Status = (AttendanceStatusEnum)item.Status
                        });
                    }
                    else
                    {
                        attendance.Status = (AttendanceStatusEnum)item.Status;
                    }
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task UpdateSessionDetail(UpdateSessionDto updateSession)
        {
            var model = await _context.ClassSessions
                .Where(x => x.Id == updateSession.Id && !x.IsDeleted)
                .FirstOrDefaultAsync();
            if (model == null)
                throw new Exception("Session is not found");
            model.SessionName = updateSession.SessionName;
            model.SessionDate = updateSession.SessionDate.ToUtcDb();
            model.Status = updateSession.Status;
            model.StartTime = updateSession.StartTime;
            model.EndTime = updateSession.EndTime;
            model.Note = updateSession.Note;
            model.FeedBack = updateSession.FeedBack;
            model.UpdatedBy = _currentUser.FullName;
            model.UpdatedDate = DateTimeOffset.UtcNow.ToUtcDb();
            await _context.SaveChangesAsync();
        }
    }
}
