using Panda.DynamicWebApi;

namespace HaoKao.Common.ModuleOptionsAction;

public class DynamicWebApiModuleOptionsAction : IDynamicWebApiModuleOptionsAction
{
    public void OptionsAction(DynamicWebApiOptions optionsAction)
    {
        optionsAction.RemoveControllerPostfixes.Clear();
    }
}