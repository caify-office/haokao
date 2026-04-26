using HaoKao.AgreementService.Domain.Entities;
using HaoKao.AgreementService.Domain.Repositories;

namespace HaoKao.AgreementService.Infrastructure.Repositories;

public class StudentAgreementRepository : Repository<StudentAgreement>, IStudentAgreementRepository;