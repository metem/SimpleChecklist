﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SimpleChecklist.LegacyDataRepository {
    using System;
    using System.Reflection;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class AppSettingsLegacy {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal AppSettingsLegacy() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("SimpleChecklist.LegacyDataRepository.AppSettingsLegacy", typeof(AppSettingsLegacy).GetTypeInfo().Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
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
        ///   Looks up a localized string similar to .btd.
        /// </summary>
        internal static string BackupFileExtension {
            get {
                return ResourceManager.GetString("BackupFileExtension", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ChecklistBackup.
        /// </summary>
        internal static string BackupFileName {
            get {
                return ResourceManager.GetString("BackupFileName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to HH:mm.
        /// </summary>
        internal static string DoneItemFinishTimeFormat {
            get {
                return ResourceManager.GetString("DoneItemFinishTimeFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to donelist.xml.
        /// </summary>
        internal static string DoneListFileName {
            get {
                return ResourceManager.GetString("DoneListFileName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to tasklist.xml.
        /// </summary>
        internal static string TaskListFileName {
            get {
                return ResourceManager.GetString("TaskListFileName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to .txt.
        /// </summary>
        internal static string TextFileExtension {
            get {
                return ResourceManager.GetString("TextFileExtension", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 1.1.
        /// </summary>
        internal static string VersionNumber {
            get {
                return ResourceManager.GetString("VersionNumber", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to www.facebook.com/SimpleChecklist.
        /// </summary>
        internal static string WebsiteUrl {
            get {
                return ResourceManager.GetString("WebsiteUrl", resourceCulture);
            }
        }
    }
}
