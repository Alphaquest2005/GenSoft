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
    #region Entities
    
    /// <summary>
    /// No Modeled Description Available
    /// </summary>
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.LightSwitch.BuildTasks.CodeGen", "14.0.0.0")]
    public sealed partial class FunctionOperation : global::Microsoft.LightSwitch.Framework.Base.EntityObject<global::LightSwitchApplication.FunctionOperation, global::LightSwitchApplication.FunctionOperation.DetailsClass>
    {
        #region Constructors
    
        /// <summary>
        /// Initializes a new instance of the FunctionOperation entity.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.LightSwitch.BuildTasks.CodeGen", "14.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public FunctionOperation()
            : this(null)
        {
        }
    
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.LightSwitch.BuildTasks.CodeGen", "14.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public FunctionOperation(global::Microsoft.LightSwitch.Framework.EntitySet<global::LightSwitchApplication.FunctionOperation> entitySet)
            : base(entitySet)
        {
            global::LightSwitchApplication.FunctionOperation.DetailsClass.Initialize(this);
        }
    
        [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        partial void FunctionOperation_Created();
        [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        partial void FunctionOperation_AllowSaveWithErrors(ref bool result);
    
        #endregion
    
        #region Private Properties
        
        /// <summary>
        /// Gets the Application object for this application.  The Application object provides access to active screens, methods to open screens and access to the current user.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.LightSwitch.BuildTasks.CodeGen", "14.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        private global::Microsoft.LightSwitch.IApplication<global::LightSwitchApplication.DataWorkspace> Application
        {
            get
            {
                return (global::Microsoft.LightSwitch.IApplication<global::LightSwitchApplication.DataWorkspace>)global::LightSwitchApplication.Application.Current;
            }
        }
        
        /// <summary>
        /// Gets the containing data workspace.  The data workspace provides access to all data sources in the application.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.LightSwitch.BuildTasks.CodeGen", "14.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        private global::LightSwitchApplication.DataWorkspace DataWorkspace
        {
            get
            {
                return (global::LightSwitchApplication.DataWorkspace)this.Details.EntitySet.Details.DataService.Details.DataWorkspace;
            }
        }
        
        #endregion
    
        #region Public Properties
    
        /// <summary>
        /// No Modeled Description Available
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.LightSwitch.BuildTasks.CodeGen", "14.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public int Id
        {
            get
            {
                return global::LightSwitchApplication.FunctionOperation.DetailsClass.GetValue(this, global::LightSwitchApplication.FunctionOperation.DetailsClass.PropertySetProperties.Id);
            }
            set
            {
                global::LightSwitchApplication.FunctionOperation.DetailsClass.SetValue(this, global::LightSwitchApplication.FunctionOperation.DetailsClass.PropertySetProperties.Id, value);
            }
        }
        
        [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        partial void Id_IsReadOnly(ref bool result);
        [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        partial void Id_Validate(global::Microsoft.LightSwitch.EntityValidationResultsBuilder results);
        [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        partial void Id_Changed();

        /// <summary>
        /// No Modeled Description Available
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.LightSwitch.BuildTasks.CodeGen", "14.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public global::LightSwitchApplication.c_Function c_Function
        {
            get
            {
                return global::LightSwitchApplication.FunctionOperation.DetailsClass.GetValue(this, global::LightSwitchApplication.FunctionOperation.DetailsClass.PropertySetProperties.c_Function);
            }
            set
            {
                global::LightSwitchApplication.FunctionOperation.DetailsClass.SetValue(this, global::LightSwitchApplication.FunctionOperation.DetailsClass.PropertySetProperties.c_Function, value);
            }
        }
        
        [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        partial void c_Function_IsReadOnly(ref bool result);
        [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        partial void c_Function_Validate(global::Microsoft.LightSwitch.EntityValidationResultsBuilder results);
        [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        partial void c_Function_Changed();

        /// <summary>
        /// No Modeled Description Available
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.LightSwitch.BuildTasks.CodeGen", "14.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public global::LightSwitchApplication.c_Operator c_Operator
        {
            get
            {
                return global::LightSwitchApplication.FunctionOperation.DetailsClass.GetValue(this, global::LightSwitchApplication.FunctionOperation.DetailsClass.PropertySetProperties.c_Operator);
            }
            set
            {
                global::LightSwitchApplication.FunctionOperation.DetailsClass.SetValue(this, global::LightSwitchApplication.FunctionOperation.DetailsClass.PropertySetProperties.c_Operator, value);
            }
        }
        
        [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        partial void c_Operator_IsReadOnly(ref bool result);
        [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        partial void c_Operator_Validate(global::Microsoft.LightSwitch.EntityValidationResultsBuilder results);
        [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        partial void c_Operator_Changed();

        /// <summary>
        /// No Modeled Description Available
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.LightSwitch.BuildTasks.CodeGen", "14.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public global::LightSwitchApplication.Parameter Parameter
        {
            get
            {
                return global::LightSwitchApplication.FunctionOperation.DetailsClass.GetValue(this, global::LightSwitchApplication.FunctionOperation.DetailsClass.PropertySetProperties.Parameter);
            }
            set
            {
                global::LightSwitchApplication.FunctionOperation.DetailsClass.SetValue(this, global::LightSwitchApplication.FunctionOperation.DetailsClass.PropertySetProperties.Parameter, value);
            }
        }
        
        [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        partial void Parameter_IsReadOnly(ref bool result);
        [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        partial void Parameter_Validate(global::Microsoft.LightSwitch.EntityValidationResultsBuilder results);
        [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        partial void Parameter_Changed();

        #endregion
    
        #region Details Class
    
        [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Never)]
        [global::System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible")]
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.LightSwitch.BuildTasks.CodeGen", "14.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public sealed class DetailsClass : global::Microsoft.LightSwitch.Details.Framework.Base.EntityDetails<
                global::LightSwitchApplication.FunctionOperation,
                global::LightSwitchApplication.FunctionOperation.DetailsClass,
                global::LightSwitchApplication.FunctionOperation.DetailsClass.IImplementation,
                global::LightSwitchApplication.FunctionOperation.DetailsClass.PropertySet,
                global::Microsoft.LightSwitch.Details.Framework.EntityCommandSet<global::LightSwitchApplication.FunctionOperation, global::LightSwitchApplication.FunctionOperation.DetailsClass>,
                global::Microsoft.LightSwitch.Details.Framework.EntityMethodSet<global::LightSwitchApplication.FunctionOperation, global::LightSwitchApplication.FunctionOperation.DetailsClass>>
        {
    
            static DetailsClass()
            {
                var initializeEntry = global::LightSwitchApplication.FunctionOperation.DetailsClass.PropertySetProperties.Id;
            }
    
            [global::System.Diagnostics.DebuggerBrowsable(global::System.Diagnostics.DebuggerBrowsableState.Never)]
            private static readonly global::Microsoft.LightSwitch.Details.Framework.Base.EntityDetails<global::LightSwitchApplication.FunctionOperation, global::LightSwitchApplication.FunctionOperation.DetailsClass>.Entry
                __FunctionOperationEntry = new global::Microsoft.LightSwitch.Details.Framework.Base.EntityDetails<global::LightSwitchApplication.FunctionOperation, global::LightSwitchApplication.FunctionOperation.DetailsClass>.Entry(
                    global::LightSwitchApplication.FunctionOperation.DetailsClass.__FunctionOperation_CreateNew,
                    global::LightSwitchApplication.FunctionOperation.DetailsClass.__FunctionOperation_Created,
                    global::LightSwitchApplication.FunctionOperation.DetailsClass.__FunctionOperation_AllowSaveWithErrors);
            private static global::LightSwitchApplication.FunctionOperation __FunctionOperation_CreateNew(global::Microsoft.LightSwitch.Framework.EntitySet<global::LightSwitchApplication.FunctionOperation> es)
            {
                return new global::LightSwitchApplication.FunctionOperation(es);
            }
            private static void __FunctionOperation_Created(global::LightSwitchApplication.FunctionOperation e)
            {
                e.FunctionOperation_Created();
            }
            private static bool __FunctionOperation_AllowSaveWithErrors(global::LightSwitchApplication.FunctionOperation e)
            {
                bool result = false;
                e.FunctionOperation_AllowSaveWithErrors(ref result);
                return result;
            }
    
            public DetailsClass() : base()
            {
            }
    
            public new global::Microsoft.LightSwitch.Details.Framework.EntityCommandSet<global::LightSwitchApplication.FunctionOperation, global::LightSwitchApplication.FunctionOperation.DetailsClass> Commands
            {
                get
                {
                    return base.Commands;
                }
            }
    
            public new global::Microsoft.LightSwitch.Details.Framework.EntityMethodSet<global::LightSwitchApplication.FunctionOperation, global::LightSwitchApplication.FunctionOperation.DetailsClass> Methods
            {
                get
                {
                    return base.Methods;
                }
            }
    
            public new global::LightSwitchApplication.FunctionOperation.DetailsClass.PropertySet Properties
            {
                get
                {
                    return base.Properties;
                }
            }
    
            [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Never)]
            [global::System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible")]
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.LightSwitch.BuildTasks.CodeGen", "14.0.0.0")]
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public sealed class PropertySet : global::Microsoft.LightSwitch.Details.Framework.Base.EntityPropertySet<global::LightSwitchApplication.FunctionOperation, global::LightSwitchApplication.FunctionOperation.DetailsClass>
            {
    
                public PropertySet() : base()
                {
                }
    
                public global::Microsoft.LightSwitch.Details.Framework.EntityStorageProperty<global::LightSwitchApplication.FunctionOperation, global::LightSwitchApplication.FunctionOperation.DetailsClass, int> Id
                {
                    get
                    {
                        return base.GetItem(global::LightSwitchApplication.FunctionOperation.DetailsClass.PropertySetProperties.Id) as global::Microsoft.LightSwitch.Details.Framework.EntityStorageProperty<global::LightSwitchApplication.FunctionOperation, global::LightSwitchApplication.FunctionOperation.DetailsClass, int>;
                    }
                }
                
                public global::Microsoft.LightSwitch.Details.Framework.EntityReferenceProperty<global::LightSwitchApplication.FunctionOperation, global::LightSwitchApplication.FunctionOperation.DetailsClass, global::LightSwitchApplication.c_Function> c_Function
                {
                    get
                    {
                        return base.GetItem(global::LightSwitchApplication.FunctionOperation.DetailsClass.PropertySetProperties.c_Function) as global::Microsoft.LightSwitch.Details.Framework.EntityReferenceProperty<global::LightSwitchApplication.FunctionOperation, global::LightSwitchApplication.FunctionOperation.DetailsClass, global::LightSwitchApplication.c_Function>;
                    }
                }
                
                public global::Microsoft.LightSwitch.Details.Framework.EntityReferenceProperty<global::LightSwitchApplication.FunctionOperation, global::LightSwitchApplication.FunctionOperation.DetailsClass, global::LightSwitchApplication.c_Operator> c_Operator
                {
                    get
                    {
                        return base.GetItem(global::LightSwitchApplication.FunctionOperation.DetailsClass.PropertySetProperties.c_Operator) as global::Microsoft.LightSwitch.Details.Framework.EntityReferenceProperty<global::LightSwitchApplication.FunctionOperation, global::LightSwitchApplication.FunctionOperation.DetailsClass, global::LightSwitchApplication.c_Operator>;
                    }
                }
                
                public global::Microsoft.LightSwitch.Details.Framework.EntityReferenceProperty<global::LightSwitchApplication.FunctionOperation, global::LightSwitchApplication.FunctionOperation.DetailsClass, global::LightSwitchApplication.Parameter> Parameter
                {
                    get
                    {
                        return base.GetItem(global::LightSwitchApplication.FunctionOperation.DetailsClass.PropertySetProperties.Parameter) as global::Microsoft.LightSwitch.Details.Framework.EntityReferenceProperty<global::LightSwitchApplication.FunctionOperation, global::LightSwitchApplication.FunctionOperation.DetailsClass, global::LightSwitchApplication.Parameter>;
                    }
                }
                
            }
    
            #pragma warning disable 109
            [global::System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible")]
            public interface IImplementation : global::Microsoft.LightSwitch.Internal.IEntityImplementation
            {
                new int Id { get; set; }
                new global::Microsoft.LightSwitch.Internal.IEntityImplementation c_Function { get; set; }
                new global::Microsoft.LightSwitch.Internal.IEntityImplementation c_Operator { get; set; }
                new global::Microsoft.LightSwitch.Internal.IEntityImplementation Parameter { get; set; }
            }
            #pragma warning restore 109
    
            [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Never)]
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.LightSwitch.BuildTasks.CodeGen", "14.0.0.0")]
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            internal class PropertySetProperties
            {
    
                [global::System.Diagnostics.DebuggerBrowsable(global::System.Diagnostics.DebuggerBrowsableState.Never)]
                public static readonly global::Microsoft.LightSwitch.Details.Framework.EntityStorageProperty<global::LightSwitchApplication.FunctionOperation, global::LightSwitchApplication.FunctionOperation.DetailsClass, int>.Entry
                    Id = new global::Microsoft.LightSwitch.Details.Framework.EntityStorageProperty<global::LightSwitchApplication.FunctionOperation, global::LightSwitchApplication.FunctionOperation.DetailsClass, int>.Entry(
                        "Id",
                        global::LightSwitchApplication.FunctionOperation.DetailsClass.PropertySetProperties._Id_Stub,
                        global::LightSwitchApplication.FunctionOperation.DetailsClass.PropertySetProperties._Id_ComputeIsReadOnly,
                        global::LightSwitchApplication.FunctionOperation.DetailsClass.PropertySetProperties._Id_Validate,
                        global::LightSwitchApplication.FunctionOperation.DetailsClass.PropertySetProperties._Id_GetImplementationValue,
                        global::LightSwitchApplication.FunctionOperation.DetailsClass.PropertySetProperties._Id_SetImplementationValue,
                        global::LightSwitchApplication.FunctionOperation.DetailsClass.PropertySetProperties._Id_OnValueChanged);
                private static void _Id_Stub(global::Microsoft.LightSwitch.Details.Framework.Base.DetailsCallback<global::LightSwitchApplication.FunctionOperation.DetailsClass, global::Microsoft.LightSwitch.Details.Framework.EntityStorageProperty<global::LightSwitchApplication.FunctionOperation, global::LightSwitchApplication.FunctionOperation.DetailsClass, int>.Data> c, global::LightSwitchApplication.FunctionOperation.DetailsClass d, object sf)
                {
                    c(d, ref d._Id, sf);
                }
                private static bool _Id_ComputeIsReadOnly(global::LightSwitchApplication.FunctionOperation e)
                {
                    bool result = false;
                    e.Id_IsReadOnly(ref result);
                    return result;
                }
                private static void _Id_Validate(global::LightSwitchApplication.FunctionOperation e, global::Microsoft.LightSwitch.EntityValidationResultsBuilder r)
                {
                    e.Id_Validate(r);
                }
                private static int _Id_GetImplementationValue(global::LightSwitchApplication.FunctionOperation.DetailsClass d)
                {
                    return d.ImplementationEntity.Id;
                }
                private static void _Id_SetImplementationValue(global::LightSwitchApplication.FunctionOperation.DetailsClass d, int v)
                {
                    d.ImplementationEntity.Id = v;
                }
                private static void _Id_OnValueChanged(global::LightSwitchApplication.FunctionOperation e)
                {
                    e.Id_Changed();
                }
    
                [global::System.Diagnostics.DebuggerBrowsable(global::System.Diagnostics.DebuggerBrowsableState.Never)]
                public static readonly global::Microsoft.LightSwitch.Details.Framework.EntityReferenceProperty<global::LightSwitchApplication.FunctionOperation, global::LightSwitchApplication.FunctionOperation.DetailsClass, global::LightSwitchApplication.c_Function>.Entry
                    c_Function = new global::Microsoft.LightSwitch.Details.Framework.EntityReferenceProperty<global::LightSwitchApplication.FunctionOperation, global::LightSwitchApplication.FunctionOperation.DetailsClass, global::LightSwitchApplication.c_Function>.Entry(
                        "c_Function",
                        global::LightSwitchApplication.FunctionOperation.DetailsClass.PropertySetProperties._c_Function_Stub,
                        global::LightSwitchApplication.FunctionOperation.DetailsClass.PropertySetProperties._c_Function_ComputeIsReadOnly,
                        global::LightSwitchApplication.FunctionOperation.DetailsClass.PropertySetProperties._c_Function_Validate,
                        global::LightSwitchApplication.FunctionOperation.DetailsClass.PropertySetProperties._c_Function_GetCoreImplementationValue,
                        global::LightSwitchApplication.FunctionOperation.DetailsClass.PropertySetProperties._c_Function_GetImplementationValue,
                        global::LightSwitchApplication.FunctionOperation.DetailsClass.PropertySetProperties._c_Function_SetImplementationValue,
                        global::LightSwitchApplication.FunctionOperation.DetailsClass.PropertySetProperties._c_Function_Refresh,
                        global::LightSwitchApplication.FunctionOperation.DetailsClass.PropertySetProperties._c_Function_OnValueChanged);
                private static void _c_Function_Stub(global::Microsoft.LightSwitch.Details.Framework.Base.DetailsCallback<global::LightSwitchApplication.FunctionOperation.DetailsClass, global::Microsoft.LightSwitch.Details.Framework.EntityReferenceProperty<global::LightSwitchApplication.FunctionOperation, global::LightSwitchApplication.FunctionOperation.DetailsClass, global::LightSwitchApplication.c_Function>.Data> c, global::LightSwitchApplication.FunctionOperation.DetailsClass d, object sf)
                {
                    c(d, ref d._c_Function, sf);
                }
                private static bool _c_Function_ComputeIsReadOnly(global::LightSwitchApplication.FunctionOperation e)
                {
                    bool result = false;
                    e.c_Function_IsReadOnly(ref result);
                    return result;
                }
                private static void _c_Function_Validate(global::LightSwitchApplication.FunctionOperation e, global::Microsoft.LightSwitch.EntityValidationResultsBuilder r)
                {
                    e.c_Function_Validate(r);
                }
                private static global::Microsoft.LightSwitch.Internal.IEntityImplementation _c_Function_GetCoreImplementationValue(global::LightSwitchApplication.FunctionOperation.DetailsClass d)
                {
                    return d.ImplementationEntity.c_Function;
                }
                private static global::LightSwitchApplication.c_Function _c_Function_GetImplementationValue(global::LightSwitchApplication.FunctionOperation.DetailsClass d)
                {
                    return d.GetImplementationValue<global::LightSwitchApplication.c_Function, global::LightSwitchApplication.c_Function.DetailsClass>(global::LightSwitchApplication.FunctionOperation.DetailsClass.PropertySetProperties.c_Function, ref d._c_Function);
                }
                private static void _c_Function_SetImplementationValue(global::LightSwitchApplication.FunctionOperation.DetailsClass d, global::LightSwitchApplication.c_Function v)
                {
                    d.SetImplementationValue(global::LightSwitchApplication.FunctionOperation.DetailsClass.PropertySetProperties.c_Function, ref d._c_Function, (i, ev) => i.c_Function = ev, v);
                }
                private static void _c_Function_Refresh(global::LightSwitchApplication.FunctionOperation.DetailsClass d)
                {
                    d.RefreshNavigationProperty(global::LightSwitchApplication.FunctionOperation.DetailsClass.PropertySetProperties.c_Function, ref d._c_Function);
                }
                private static void _c_Function_OnValueChanged(global::LightSwitchApplication.FunctionOperation e)
                {
                    e.c_Function_Changed();
                }
    
                [global::System.Diagnostics.DebuggerBrowsable(global::System.Diagnostics.DebuggerBrowsableState.Never)]
                public static readonly global::Microsoft.LightSwitch.Details.Framework.EntityReferenceProperty<global::LightSwitchApplication.FunctionOperation, global::LightSwitchApplication.FunctionOperation.DetailsClass, global::LightSwitchApplication.c_Operator>.Entry
                    c_Operator = new global::Microsoft.LightSwitch.Details.Framework.EntityReferenceProperty<global::LightSwitchApplication.FunctionOperation, global::LightSwitchApplication.FunctionOperation.DetailsClass, global::LightSwitchApplication.c_Operator>.Entry(
                        "c_Operator",
                        global::LightSwitchApplication.FunctionOperation.DetailsClass.PropertySetProperties._c_Operator_Stub,
                        global::LightSwitchApplication.FunctionOperation.DetailsClass.PropertySetProperties._c_Operator_ComputeIsReadOnly,
                        global::LightSwitchApplication.FunctionOperation.DetailsClass.PropertySetProperties._c_Operator_Validate,
                        global::LightSwitchApplication.FunctionOperation.DetailsClass.PropertySetProperties._c_Operator_GetCoreImplementationValue,
                        global::LightSwitchApplication.FunctionOperation.DetailsClass.PropertySetProperties._c_Operator_GetImplementationValue,
                        global::LightSwitchApplication.FunctionOperation.DetailsClass.PropertySetProperties._c_Operator_SetImplementationValue,
                        global::LightSwitchApplication.FunctionOperation.DetailsClass.PropertySetProperties._c_Operator_Refresh,
                        global::LightSwitchApplication.FunctionOperation.DetailsClass.PropertySetProperties._c_Operator_OnValueChanged);
                private static void _c_Operator_Stub(global::Microsoft.LightSwitch.Details.Framework.Base.DetailsCallback<global::LightSwitchApplication.FunctionOperation.DetailsClass, global::Microsoft.LightSwitch.Details.Framework.EntityReferenceProperty<global::LightSwitchApplication.FunctionOperation, global::LightSwitchApplication.FunctionOperation.DetailsClass, global::LightSwitchApplication.c_Operator>.Data> c, global::LightSwitchApplication.FunctionOperation.DetailsClass d, object sf)
                {
                    c(d, ref d._c_Operator, sf);
                }
                private static bool _c_Operator_ComputeIsReadOnly(global::LightSwitchApplication.FunctionOperation e)
                {
                    bool result = false;
                    e.c_Operator_IsReadOnly(ref result);
                    return result;
                }
                private static void _c_Operator_Validate(global::LightSwitchApplication.FunctionOperation e, global::Microsoft.LightSwitch.EntityValidationResultsBuilder r)
                {
                    e.c_Operator_Validate(r);
                }
                private static global::Microsoft.LightSwitch.Internal.IEntityImplementation _c_Operator_GetCoreImplementationValue(global::LightSwitchApplication.FunctionOperation.DetailsClass d)
                {
                    return d.ImplementationEntity.c_Operator;
                }
                private static global::LightSwitchApplication.c_Operator _c_Operator_GetImplementationValue(global::LightSwitchApplication.FunctionOperation.DetailsClass d)
                {
                    return d.GetImplementationValue<global::LightSwitchApplication.c_Operator, global::LightSwitchApplication.c_Operator.DetailsClass>(global::LightSwitchApplication.FunctionOperation.DetailsClass.PropertySetProperties.c_Operator, ref d._c_Operator);
                }
                private static void _c_Operator_SetImplementationValue(global::LightSwitchApplication.FunctionOperation.DetailsClass d, global::LightSwitchApplication.c_Operator v)
                {
                    d.SetImplementationValue(global::LightSwitchApplication.FunctionOperation.DetailsClass.PropertySetProperties.c_Operator, ref d._c_Operator, (i, ev) => i.c_Operator = ev, v);
                }
                private static void _c_Operator_Refresh(global::LightSwitchApplication.FunctionOperation.DetailsClass d)
                {
                    d.RefreshNavigationProperty(global::LightSwitchApplication.FunctionOperation.DetailsClass.PropertySetProperties.c_Operator, ref d._c_Operator);
                }
                private static void _c_Operator_OnValueChanged(global::LightSwitchApplication.FunctionOperation e)
                {
                    e.c_Operator_Changed();
                }
    
                [global::System.Diagnostics.DebuggerBrowsable(global::System.Diagnostics.DebuggerBrowsableState.Never)]
                public static readonly global::Microsoft.LightSwitch.Details.Framework.EntityReferenceProperty<global::LightSwitchApplication.FunctionOperation, global::LightSwitchApplication.FunctionOperation.DetailsClass, global::LightSwitchApplication.Parameter>.Entry
                    Parameter = new global::Microsoft.LightSwitch.Details.Framework.EntityReferenceProperty<global::LightSwitchApplication.FunctionOperation, global::LightSwitchApplication.FunctionOperation.DetailsClass, global::LightSwitchApplication.Parameter>.Entry(
                        "Parameter",
                        global::LightSwitchApplication.FunctionOperation.DetailsClass.PropertySetProperties._Parameter_Stub,
                        global::LightSwitchApplication.FunctionOperation.DetailsClass.PropertySetProperties._Parameter_ComputeIsReadOnly,
                        global::LightSwitchApplication.FunctionOperation.DetailsClass.PropertySetProperties._Parameter_Validate,
                        global::LightSwitchApplication.FunctionOperation.DetailsClass.PropertySetProperties._Parameter_GetCoreImplementationValue,
                        global::LightSwitchApplication.FunctionOperation.DetailsClass.PropertySetProperties._Parameter_GetImplementationValue,
                        global::LightSwitchApplication.FunctionOperation.DetailsClass.PropertySetProperties._Parameter_SetImplementationValue,
                        global::LightSwitchApplication.FunctionOperation.DetailsClass.PropertySetProperties._Parameter_Refresh,
                        global::LightSwitchApplication.FunctionOperation.DetailsClass.PropertySetProperties._Parameter_OnValueChanged);
                private static void _Parameter_Stub(global::Microsoft.LightSwitch.Details.Framework.Base.DetailsCallback<global::LightSwitchApplication.FunctionOperation.DetailsClass, global::Microsoft.LightSwitch.Details.Framework.EntityReferenceProperty<global::LightSwitchApplication.FunctionOperation, global::LightSwitchApplication.FunctionOperation.DetailsClass, global::LightSwitchApplication.Parameter>.Data> c, global::LightSwitchApplication.FunctionOperation.DetailsClass d, object sf)
                {
                    c(d, ref d._Parameter, sf);
                }
                private static bool _Parameter_ComputeIsReadOnly(global::LightSwitchApplication.FunctionOperation e)
                {
                    bool result = false;
                    e.Parameter_IsReadOnly(ref result);
                    return result;
                }
                private static void _Parameter_Validate(global::LightSwitchApplication.FunctionOperation e, global::Microsoft.LightSwitch.EntityValidationResultsBuilder r)
                {
                    e.Parameter_Validate(r);
                }
                private static global::Microsoft.LightSwitch.Internal.IEntityImplementation _Parameter_GetCoreImplementationValue(global::LightSwitchApplication.FunctionOperation.DetailsClass d)
                {
                    return d.ImplementationEntity.Parameter;
                }
                private static global::LightSwitchApplication.Parameter _Parameter_GetImplementationValue(global::LightSwitchApplication.FunctionOperation.DetailsClass d)
                {
                    return d.GetImplementationValue<global::LightSwitchApplication.Parameter, global::LightSwitchApplication.Parameter.DetailsClass>(global::LightSwitchApplication.FunctionOperation.DetailsClass.PropertySetProperties.Parameter, ref d._Parameter);
                }
                private static void _Parameter_SetImplementationValue(global::LightSwitchApplication.FunctionOperation.DetailsClass d, global::LightSwitchApplication.Parameter v)
                {
                    d.SetImplementationValue(global::LightSwitchApplication.FunctionOperation.DetailsClass.PropertySetProperties.Parameter, ref d._Parameter, (i, ev) => i.Parameter = ev, v);
                }
                private static void _Parameter_Refresh(global::LightSwitchApplication.FunctionOperation.DetailsClass d)
                {
                    d.RefreshNavigationProperty(global::LightSwitchApplication.FunctionOperation.DetailsClass.PropertySetProperties.Parameter, ref d._Parameter);
                }
                private static void _Parameter_OnValueChanged(global::LightSwitchApplication.FunctionOperation e)
                {
                    e.Parameter_Changed();
                }
    
            }
    
            [global::System.Diagnostics.DebuggerBrowsable(global::System.Diagnostics.DebuggerBrowsableState.Never)]
            private global::Microsoft.LightSwitch.Details.Framework.EntityStorageProperty<global::LightSwitchApplication.FunctionOperation, global::LightSwitchApplication.FunctionOperation.DetailsClass, int>.Data _Id;
            
            [global::System.Diagnostics.DebuggerBrowsable(global::System.Diagnostics.DebuggerBrowsableState.Never)]
            private global::Microsoft.LightSwitch.Details.Framework.EntityReferenceProperty<global::LightSwitchApplication.FunctionOperation, global::LightSwitchApplication.FunctionOperation.DetailsClass, global::LightSwitchApplication.c_Function>.Data _c_Function;
            
            [global::System.Diagnostics.DebuggerBrowsable(global::System.Diagnostics.DebuggerBrowsableState.Never)]
            private global::Microsoft.LightSwitch.Details.Framework.EntityReferenceProperty<global::LightSwitchApplication.FunctionOperation, global::LightSwitchApplication.FunctionOperation.DetailsClass, global::LightSwitchApplication.c_Operator>.Data _c_Operator;
            
            [global::System.Diagnostics.DebuggerBrowsable(global::System.Diagnostics.DebuggerBrowsableState.Never)]
            private global::Microsoft.LightSwitch.Details.Framework.EntityReferenceProperty<global::LightSwitchApplication.FunctionOperation, global::LightSwitchApplication.FunctionOperation.DetailsClass, global::LightSwitchApplication.Parameter>.Data _Parameter;
            
        }
    
        #endregion
    }
    
    #endregion
}
