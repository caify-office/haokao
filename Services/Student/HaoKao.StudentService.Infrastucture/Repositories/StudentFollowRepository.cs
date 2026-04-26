using HaoKao.StudentService.Domain.Entities;
using HaoKao.StudentService.Domain.Repositories;

namespace HaoKao.StudentService.Infrastructure.Repositories;

public class StudentFollowRepository : Repository<StudentFollow>, IStudentFollowRepository;