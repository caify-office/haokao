namespace HaoKao.ChapterNodeService.Domain.ChapterNodeModule;

public interface IChapterNodeRepository : IRepository<ChapterNode>
{
     Task<List<ChapterNode>> GetByQueryAsync(QueryBase<ChapterNode> query);

     Task<ChapterNode> GetBaseParentChapterNode(Guid id);

    Task<List<ChapterNode>> GetChapterNodeList(Guid subjectId);
    Task<List<ChapterNode>> GetChapterNodeListByParentId(Guid parentId);

    Task<Dictionary<Guid, string>> GetChapterDictionary(Guid subjectId);
    Task<Guid[]> GetAllChildrenChapterNodeId(Guid id);
    Task<List<ChapterNodeTreeCacheItem>> GetChapterNodeTreeByQueryAsync(Guid? subjectId, Guid? id);
    Task<List<ChapterNode>> GetChapterNodeKnowledgePointTree(Guid subjectId);
}

// 定义具体的实体类
public record ChapterNodeTreeCacheItem(
    string Id,
    string Code,
    string Name,
    string ParentId,
    string ParentName,
    int Sort,
    bool IsLeaf
);