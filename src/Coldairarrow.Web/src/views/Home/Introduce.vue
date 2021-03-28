<template>
  <a-card :bordered="false">
    <div>
      <p>本框架为.NET Core5.0+Wpf（CS-重点）/Ant Design Vue版（此处不在介绍）/Vant(手机端-稍微介绍下) </p>
      <p>本框架旨在极大的提高开发效率</p>
      <p>
        使用技术栈：<br />
        <br />
        后端：由Colder.Admin.AntdVue改造完成的，请参照（本处不在介绍，请看原作者的介绍，本文重点为Wpf）<br />
        <a
          href="https://github.com/Coldairarrow/Colder.Admin.AntdVue"
          target="_blank"
        >https://github.com/Coldairarrow/Colder.Admin.AntdVue</a>

        另外扩展了Quartz，WebSocket实现即时聊天及实时消息推送，WorkflowCore实现了OA流程。<br />
        <br />
        前端：Wpf
        <br />
        安装包下载地址：
        <br />
        <a href="http://121.36.12.76:5000/setup/" target="_blank">http://121.36.12.76:5000/setup</a>
        <br />
        自动升级地址:
        <br />
        <a href="http://121.36.12.76:5000/update/" target="_blank">http://121.36.12.76:5000/update</a>
        <br />
        具体技术实施：<br />
        项目采用Prism的MVVM，并采用严格分层模式，极大的增加聚合度，降低耦合度，完全兼容BS端，使用相同的API接口。<br />
        1.AIStudio.Wpf.Service与后台交互的逻辑。<br />
        2.本项目使用MahApps.Metro.NET45的扁平风格，并集成了Xceed.Wpf.ToolKit，HandyControl，MaterialDesignInXamlToolkit等其它优秀框架的控件，并且修改成MahApps.Metro.NET45的统一主题。
        <br />
        3.控件的使用可以在AIStudio.Wpf.DemoPage中查看，并且支持运行时代码复制。<br />
        4.代码生成器，可以生成直接可运行的界面，加快开发速度。<br />
        <br />
        相关链接:<br />
        本框架链接,欢迎点星支持:<a
          href="https://gitee.com/akwkevin/aistudio.-wpf.-aclient"
          target="_blank"
        >https://gitee.com/akwkevin/aistudio.-wpf.-aclient</a
        ><br />
        本框架Vue链接,欢迎点星支持:<a
          href="https://gitee.com/akwkevin/aistudio.-vue.-aclient"
          target="_blank"
        >https://gitee.com/akwkevin/aistudio.-vue.-aclient</a
        ><br />
        个人gitee,欢迎点星支持:<a
          href="https://gitee.com/akwkevin"
          target="_blank"
        >https://gitee.com/akwkevin</a
        ><br />
        个人博客:<a
          href="https://www.cnblogs.com/akwkevin/"
          target="_blank"
        >https://www.cnblogs.com/akwkevin/</a
        ><br />
        .NET Core3.0 下载:<a
          href="https://dotnet.microsoft.com/download"
          target="_blank"
        >https://dotnet.microsoft.com/download</a
        ><br />
        MahApps.Metro:<a href="https://mahapps.com/" target="_blank">https://mahapps.com/</a><br />
        HandyControl:<a
          href="https://handyorg.github.io/handycontrol/"
          target="_blank"
        >https://handyorg.github.io/handycontrol/</a
        ><br />
        Xceed.Wpf:<a href="https://github.com/xceedsoftware/wpftoolkit/" target="_blank">https://github.com/xceedsoftware/wpftoolkit/</a><br /><br />
        个人QQ:80267720(欢迎技术交流及技术合作)<br />
        QQ技术交流群:51286643
      </p>
    </div>
  </a-card>
</template>
