namespace HaoKao.BasicService.Domain.Entities;

[Serializable]
public abstract class PermissionBase : AggregateRoot<Guid>
{
    #region Private Members

    //Permission
    private Permission _allowMask = 0;
    private Permission _denyMask = (Permission)(-1);

    #endregion

    public virtual Role Role => new();

    #region Core Public Properties

    [XmlAttribute(AttributeName = "allowMask", DataType = "long")]
    public virtual Permission AllowMask
    {
        get => _allowMask;
        set => _allowMask = value;
    }

    [XmlAttribute(AttributeName = "denyMask", DataType = "long")]
    public virtual Permission DenyMask
    {
        get => _denyMask;
        set => _denyMask = value;
    }

    #endregion

    #region GetBit

    public virtual bool GetBit(Permission mask)
    {
        var bReturn = false;

        if ((_allowMask & mask) == mask) bReturn = true;

        if ((_denyMask & mask) == mask) bReturn = false;

        return bReturn;
    }

    #endregion

    #region Public Methods

    public virtual void SetBit(Permission mask, AccessControlEntry accessControl)
    {
        switch (accessControl)
        {
            case AccessControlEntry.Allow:
                _allowMask |= (Permission)((long)mask & -1);
                _denyMask &= ~(Permission)((long)mask & -1);
                break;

            case AccessControlEntry.NotSet:
                _allowMask &= ~(Permission)((long)mask & -1);
                _denyMask &= ~(Permission)((long)mask & -1);
                break;

            default:
                _allowMask &= ~(Permission)((long)mask & -1);
                _denyMask |= (Permission)((long)mask & -1);
                break;
        }
    }

    public virtual void Merge(Permission p)
    {
        _allowMask |= p;
        _denyMask &= p;
    }

    public virtual void Merge(PermissionBase permissionBase)
    {
        _allowMask |= permissionBase.AllowMask;
        _denyMask |= permissionBase.DenyMask;
    }

    public override bool Equals(object obj)
    {
        var isEqual = true;
        var permissionBase = obj as PermissionBase;
        if (permissionBase == null && this != null) return isEqual;

        foreach (Permission permission in Enum.GetValues<Permission>())
        {
            if (permissionBase.GetBit(permission) != GetBit(permission))
            {
                isEqual = false;
                break;
            }
        }

        return isEqual;
    }

    #endregion
}