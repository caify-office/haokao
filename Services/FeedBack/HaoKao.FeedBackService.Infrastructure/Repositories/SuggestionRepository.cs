using HaoKao.FeedBackService.Domain.Entities;
using HaoKao.FeedBackService.Domain.Repositories;

namespace HaoKao.FeedBackService.Infrastructure.Repositories;

public class SuggestionRepository : Repository<Suggestion>, ISuggestionRepository;