﻿


//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace LightSwitchApplication
{
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.LightSwitch.BuildTasks.CodeGen", "14.0.0.0")]
    public sealed partial class Application
        : global::Microsoft.LightSwitch.Framework.Client.ClientApplication<global::LightSwitchApplication.Application, global::LightSwitchApplication.Application.DetailsClass, global::LightSwitchApplication.DataWorkspace>
    {
        [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Never)]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public Application(global::Microsoft.LightSwitch.Model.IApplicationDefinition applicationDefinition) : base(applicationDefinition)
        {
        }

        [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        partial void Application_Initialize();
        [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        partial void Application_LoggedIn();

        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public static new global::LightSwitchApplication.Application Current
        {
            get
            {
                return (global::LightSwitchApplication.Application)global::Microsoft.LightSwitch.Framework.Client.ClientApplication<global::LightSwitchApplication.Application, global::LightSwitchApplication.Application.DetailsClass>.Current;
            }
        }

        [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        partial void Application1Detail_CanRun(ref bool result, int Application1Id);
        [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        partial void Application1Detail_Run(ref bool handled, int Application1Id);
    
        /// <summary>
        /// Opens the ShowApplication1Detail screen.  If the screen is already opened, it is activated and shown.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.LightSwitch.BuildTasks.CodeGen", "14.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void ShowApplication1Detail(int Application1Id)
        {
            ((global::Microsoft.LightSwitch.Details.Client.IClientApplicationDetails)this.Details).InvokeMethod(this.Details.Methods.ShowApplication1Detail, Application1Id);
        }
        
        [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Never)]
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.LightSwitch.BuildTasks.CodeGen", "14.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public sealed class DetailsClass
            : global::Microsoft.LightSwitch.Details.Framework.Client.ClientApplicationDetails<global::LightSwitchApplication.Application, global::LightSwitchApplication.Application.DetailsClass, global::LightSwitchApplication.Application.DetailsClass.PropertySet, global::LightSwitchApplication.Application.DetailsClass.CommandSet, global::LightSwitchApplication.Application.DetailsClass.MethodSet>
        {

            static DetailsClass()
            {
                var initializeMethodEntry = global::LightSwitchApplication.Application.DetailsClass.MethodSetProperties.ShowApplication1Detail;
            }

            [global::System.Diagnostics.DebuggerBrowsable(global::System.Diagnostics.DebuggerBrowsableState.Never)]
            private static readonly global::Microsoft.LightSwitch.Details.Framework.Base.ApplicationDetails<global::LightSwitchApplication.Application, global::LightSwitchApplication.Application.DetailsClass>.Entry
                __ApplicationEntry = new global::Microsoft.LightSwitch.Details.Framework.Client.ClientApplicationDetails<global::LightSwitchApplication.Application, global::LightSwitchApplication.Application.DetailsClass>.Entry(
                    global::LightSwitchApplication.Application.DetailsClass.__Application_Initialized,
                    global::LightSwitchApplication.Application.DetailsClass.__Application_LoggedIn);
            private static void __Application_Initialized(global::LightSwitchApplication.Application a)
            {
                a.Application_Initialize();
            }
            private static void __Application_LoggedIn(global::LightSwitchApplication.Application a)
            {
                a.Application_LoggedIn();
            }

            public DetailsClass() : base()
            {
            }

            public new global::LightSwitchApplication.Application.DetailsClass.PropertySet Properties
            {
                get
                {
                    return base.Properties;
                }
            }

            public new global::LightSwitchApplication.Application.DetailsClass.CommandSet Commands
            {
                get
                {
                    return base.Commands;
                }
            }

            public new global::LightSwitchApplication.Application.DetailsClass.MethodSet Methods
            {
                get
                {
                    return base.Methods;
                }
            }

            protected override global::Microsoft.LightSwitch.Client.IScreenObject CreateScreen(string screenName, params object[] args)
            {
                switch (screenName)
                {
                    case "Application1Detail":
                        return global::LightSwitchApplication.Application1Detail.CreateInstance((int)args[0]);
                }
            
                return base.CreateScreen(screenName, args);
            }
            
            [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Never)]
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.LightSwitch.BuildTasks.CodeGen", "14.0.0.0")]
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public sealed class PropertySet
                : global::Microsoft.LightSwitch.Details.Framework.Client.ClientApplicationPropertySet<global::LightSwitchApplication.Application, global::LightSwitchApplication.Application.DetailsClass>
            {

            }

            [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Never)]
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.LightSwitch.BuildTasks.CodeGen", "14.0.0.0")]
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public sealed class CommandSet
                : global::Microsoft.LightSwitch.Details.Framework.Client.ClientApplicationCommandSet<global::LightSwitchApplication.Application, global::LightSwitchApplication.Application.DetailsClass>
            {

            }

            [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Never)]
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.LightSwitch.BuildTasks.CodeGen", "14.0.0.0")]
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public sealed class MethodSet
                : global::Microsoft.LightSwitch.Details.Framework.Client.ClientApplicationMethodSet<global::LightSwitchApplication.Application, global::LightSwitchApplication.Application.DetailsClass>
            {

                public global::Microsoft.LightSwitch.Details.Framework.ApplicationMethod<global::LightSwitchApplication.Application, global::LightSwitchApplication.Application.DetailsClass> ShowApplication1Detail
                {
                    get
                    {
                        return (global::Microsoft.LightSwitch.Details.Framework.ApplicationMethod<global::LightSwitchApplication.Application, global::LightSwitchApplication.Application.DetailsClass>)
                               base.GetItem(global::LightSwitchApplication.Application.DetailsClass.MethodSetProperties.ShowApplication1Detail);
                    }
                }

            }

            [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Never)]
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.LightSwitch.BuildTasks.CodeGen", "14.0.0.0")]
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            internal sealed class PropertySetProperties
            {

            }

            [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Never)]
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.LightSwitch.BuildTasks.CodeGen", "14.0.0.0")]
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            internal sealed class CommandSetProperties
            {

            }

            [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Never)]
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.LightSwitch.BuildTasks.CodeGen", "14.0.0.0")]
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            internal sealed class MethodSetProperties
            {

                public static readonly global::Microsoft.LightSwitch.Details.Framework.ApplicationMethod<global::LightSwitchApplication.Application, global::LightSwitchApplication.Application.DetailsClass>.Entry
                    ShowApplication1Detail = new global::Microsoft.LightSwitch.Details.Framework.ApplicationMethod<global::LightSwitchApplication.Application, global::LightSwitchApplication.Application.DetailsClass>.Entry(
                        "ShowApplication1Detail",
                        global::LightSwitchApplication.Application.DetailsClass.MethodSetProperties._ShowApplication1Detail_Stub,
                        global::LightSwitchApplication.Application.DetailsClass.MethodSetProperties._ShowApplication1Detail_CanInvoke,
                        global::LightSwitchApplication.Application.DetailsClass.MethodSetProperties._ShowApplication1Detail_InvokeMethod);
                private static void _ShowApplication1Detail_Stub(global::Microsoft.LightSwitch.Details.Framework.Base.DetailsCallback<global::LightSwitchApplication.Application.DetailsClass, global::Microsoft.LightSwitch.Details.Framework.ApplicationMethod<global::LightSwitchApplication.Application, global::LightSwitchApplication.Application.DetailsClass>.Data> c, global::LightSwitchApplication.Application.DetailsClass d, object sf)
                {
                    c(d, ref d._ShowApplication1DetailMethod, sf);
                }
                private static global::System.Exception _ShowApplication1Detail_CanInvoke(global::LightSwitchApplication.Application.DetailsClass d, global::System.Collections.ObjectModel.ReadOnlyCollection<object> args, global::System.Exception ex)
                {
                    bool result = true;
                    d.Application.Application1Detail_CanRun(ref result, (int)args[0]);
                    return result ? null : ex;
                }
                private static void _ShowApplication1Detail_InvokeMethod(global::LightSwitchApplication.Application.DetailsClass d, global::System.Collections.ObjectModel.ReadOnlyCollection<object> args)
                {
                    bool handled = false;
                    d.Application.Application1Detail_Run(ref handled, (int)args[0]);
                    if (!handled)
                    {
                        d.ShowScreen("LightSwitchApplication.RevolutionRawInterface.DesktopClient:Application1Detail", () => global::LightSwitchApplication.Application1Detail.CreateInstance((int)args[0]), args);
                    }
                }
 
            }

            private global::Microsoft.LightSwitch.Details.Framework.ApplicationMethod<global::LightSwitchApplication.Application, global::LightSwitchApplication.Application.DetailsClass>.Data _ShowApplication1DetailMethod;

        }
    }

    [global::System.ComponentModel.Composition.Export(typeof(global::Microsoft.LightSwitch.Model.IModuleDefinitionLoader))]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.LightSwitch.BuildTasks.CodeGen", "14.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Never)]
    [global::Microsoft.LightSwitch.Model.ModuleDefinitionLoader("LightSwitchApplication.RevolutionRawInterface.DesktopClient")]
    public class ClientModuleLoader : global::Microsoft.LightSwitch.Model.IModuleDefinitionLoader
    {
        private bool resourceManagerChecked;
        private global::System.Resources.ResourceManager resourceManager;
    
        public global::System.Resources.ResourceManager GetModelResourceManager()
        {
            if (resourceManagerChecked == false)
            {
                global::System.Reflection.Assembly assembly = global::System.Reflection.Assembly.GetExecutingAssembly();
                
                foreach (string resourceName in assembly.GetManifestResourceNames()) 
                {
                    if (resourceName.EndsWith("Client.resources", global::System.StringComparison.OrdinalIgnoreCase)) 
                    {
                        string resxName = resourceName.Substring(0, resourceName.Length - ".resources".Length);
                        resourceManager = new global::System.Resources.ResourceManager(resxName, assembly);
                        break;
                    }
                }
                resourceManagerChecked = true;
            }
    
            return resourceManager;
        }
    
        public global::System.Collections.Generic.IEnumerable<global::System.IO.Stream> LoadModelFragments()
        {
            global::System.Reflection.Assembly assembly = global::System.Reflection.Assembly.GetExecutingAssembly();
            global::System.Collections.Generic.List<global::System.IO.Stream> streams = new global::System.Collections.Generic.List<global::System.IO.Stream>();
    
            foreach (string resourceName in assembly.GetManifestResourceNames())
            {
                if (resourceName.EndsWith(".lsml", global::System.StringComparison.OrdinalIgnoreCase))
                {
                    streams.Add(assembly.GetManifestResourceStream(resourceName));
                }
            }
    
            return streams;
        }
    }
}
