using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project
{
    public class App
    {
        private readonly string filePath = System.Environment.CurrentDirectory + @"\table.xml";

        private static App thisApp = null;

        private XmlHelper xmlHelper = null;

        static App()
        {
            thisApp = new App();
        }

        private App()
        {
        }

        public static App ThisApp
        {
            get { return thisApp; }
        }

        /// <summary>
        /// 当前打开项目所关联的XML操作对象
        /// </summary>
        public XmlHelper XMLHelper
        {
            get
            {
                if (this.xmlHelper == null)
                    this.xmlHelper = new XmlHelper(filePath, "Project");
                
                return this.xmlHelper;
            }
        }

        /// <summary>
        /// 当前操作系统DPI的百分比,例如：100%、125%、150%...
        /// </summary>
        public double DPI_Percent { get; set; }
    }
}
