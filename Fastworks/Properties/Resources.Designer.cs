﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.18408
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

namespace Fastworks.Properties {
    using System;
    
    
    /// <summary>
    ///   一个强类型的资源类，用于查找本地化的字符串等。
    /// </summary>
    // 此类是由 StronglyTypedResourceBuilder
    // 类通过类似于 ResGen 或 Visual Studio 的工具自动生成的。
    // 若要添加或移除成员，请编辑 .ResX 文件，然后重新运行 ResGen
    // (以 /str 作为命令选项)，或重新生成 VS 项目。
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   返回此类使用的缓存的 ResourceManager 实例。
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Fastworks.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   使用此强类型资源类，为所有资源查找
        ///   重写当前线程的 CurrentUICulture 属性。
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   查找类似 Failed to add the aggregate(s) to the repository. 的本地化字符串。
        /// </summary>
        internal static string EX_ADD_AGGREGATE_FAIL {
            get {
                return ResourceManager.GetString("EX_ADD_AGGREGATE_FAIL", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 Failed to begin the transaction for the domain repository. 的本地化字符串。
        /// </summary>
        internal static string EX_BEGIN_TRANS_DOMAIN_REPOSITORY_FAIL {
            get {
                return ResourceManager.GetString("EX_BEGIN_TRANS_DOMAIN_REPOSITORY_FAIL", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 Failed to begin transaction with the isolation level {0}. 的本地化字符串。
        /// </summary>
        internal static string EX_BEGIN_TRANS_STORAGE_FAIL {
            get {
                return ResourceManager.GetString("EX_BEGIN_TRANS_STORAGE_FAIL", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 The expression type {0} is not supported by current version of Apworks. 的本地化字符串。
        /// </summary>
        internal static string EX_EXPRESSION_NODE_TYPE_NOT_SUPPORT {
            get {
                return ResourceManager.GetString("EX_EXPRESSION_NODE_TYPE_NOT_SUPPORT", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 Number of the arguments used by the method call should only be one. 的本地化字符串。
        /// </summary>
        internal static string EX_INVALID_METHOD_CALL_ARGUMENT_NUMBER {
            get {
                return ResourceManager.GetString("EX_INVALID_METHOD_CALL_ARGUMENT_NUMBER", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 Only the property member of the given data object and the field member is supported. Current member type is {0}. 的本地化字符串。
        /// </summary>
        internal static string EX_MEMBER_TYPE_NOT_SUPPORT {
            get {
                return ResourceManager.GetString("EX_MEMBER_TYPE_NOT_SUPPORT", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 The type of the argument that used by the method should be either Constant or Member. Currently it is {0}. 的本地化字符串。
        /// </summary>
        internal static string EX_METHOD_CALL_ARGUMENT_TYPE_NOT_SUPPORT {
            get {
                return ResourceManager.GetString("EX_METHOD_CALL_ARGUMENT_TYPE_NOT_SUPPORT", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 Only the StartsWith, EndsWith and Equals methods are supported. Currently it is {0}. 的本地化字符串。
        /// </summary>
        internal static string EX_METHOD_NOT_SUPPORT {
            get {
                return ResourceManager.GetString("EX_METHOD_NOT_SUPPORT", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 The process of the expression type {0} is not supported. 的本地化字符串。
        /// </summary>
        internal static string EX_PROCESS_NODE_NOT_SUPPORT {
            get {
                return ResourceManager.GetString("EX_PROCESS_NODE_NOT_SUPPORT", resourceCulture);
            }
        }
    }
}
