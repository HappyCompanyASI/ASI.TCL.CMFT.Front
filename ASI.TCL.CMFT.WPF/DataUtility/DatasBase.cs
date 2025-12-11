using System.Threading.Tasks;
using ASI.TCL.CMFT.WPF.Utilities;
using Prism.Ioc;

namespace ASI.TCL.CMFT.WPF.DataUtility
{
    // 泛型靜態資源基底類別
    public abstract class DatasBase<TData, TDesignTime, TRunTime >
        where TDesignTime : class, TData, new()
        where TRunTime : class, TData
    {
        public TData Datas { get; private set; }

        // 設計時資料初始化
        protected DatasBase()
        {
            if (WPFHelper.IsDesignTime)
            {
                Datas = new TDesignTime();
                if (Datas is IDesignTimeDatas designTimeStaticDatas)
                {
                    designTimeStaticDatas.InitDatas();
                }
            }
        }

        // 運行時初始化
        public async Task LoadDatasAsync()
        {
            Datas = ContainerLocator.Container.Resolve<TRunTime>();
            if (Datas is IRunTimeDatas runTimeStaticDatas)
            {
                await runTimeStaticDatas.InitDatasAsync();
            }
        }
    }
}
