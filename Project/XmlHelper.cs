using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Project
{
    /// <summary>
    /// XML文件助手
    /// </summary>
    public class XmlHelper
    {
        static char[] m_sp = { '\\', '/' };

        // XML文件路径
        private string m_xmlPath = String.Empty;
        // XML文档
        private XmlDocument m_xmlDoc = null;
        // 根节点
        private XmlElement m_xmlRootNode = null;
        // 当前节点
        private XmlElement m_xmlSelectedNode = null;

        public XmlHelper(string xmlPath, string rootNode)
        {
            this.LoadXml(xmlPath, rootNode);
        }

#region 属性
        public string XmlFilePath
        {
            get { return this.m_xmlPath; }
        }

        public XmlDocument XmlDoc
        {
            get { return this.m_xmlDoc; }
        }

        public XmlElement XmlRootNode
        {
            get { return this.m_xmlRootNode; }
        }

        public XmlElement XmlSelectedNode
        {
            get { return this.m_xmlSelectedNode; }
        }
#endregion


#region 接口
        /// <summary>
        /// 载入磁盘xml文件到内存
        /// </summary>
        /// <param name="xmlPath">xml文件路径</param>
        /// <param name="rootNode">根节点</param>
        public void LoadXml(string xmlPath, string rootNode)
        {
            if (String.IsNullOrWhiteSpace(xmlPath))
                return;
            this.m_xmlPath = xmlPath;
            this.m_xmlDoc = new XmlDocument();

            try
            {
                // 判断xml文件是否存在
                if (System.IO.File.Exists(this.m_xmlPath))
                {
                    this.m_xmlDoc.Load(this.m_xmlPath);
                    this.m_xmlRootNode = this.m_xmlDoc.DocumentElement;
                    this.m_xmlSelectedNode = this.m_xmlRootNode;
                }
                else
                {
                    // 在内存中创建Xml声明节点并添加
                    XmlNode xmlNode = this.m_xmlDoc.CreateXmlDeclaration("1.0", "UTF-8", null);
                    this.m_xmlDoc.AppendChild(xmlNode);
                    // 创建xml根节点
                    if (String.IsNullOrWhiteSpace(rootNode))
                        return;
                    XmlElement xmlRootNode = this.m_xmlDoc.CreateElement(String.Empty, rootNode, String.Empty);
                    this.m_xmlRootNode = this.m_xmlDoc.AppendChild(xmlRootNode) as XmlElement;
                    this.m_xmlSelectedNode = this.m_xmlRootNode;
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// 保存xml文件。写入xml后，要调用此方法保存
        /// </summary>
        /// <param name="xmlPath">另存路径</param>
        public void SaveXml(string xmlPath = @"")
        {
            xmlPath = String.IsNullOrEmpty(xmlPath) ? this.m_xmlPath : xmlPath;
            if (null == this.m_xmlDoc || String.IsNullOrEmpty(xmlPath))
                return;
            this.m_xmlDoc.Save(xmlPath);
        }

        /// <summary>
        /// 是否已经准备好了"读"或"写"
        /// </summary>
        /// <returns>true：准备就绪</returns>
        public bool IsReady()
        {
            return (null != this.m_xmlDoc) && (null != this.m_xmlRootNode) && (null != this.m_xmlSelectedNode);
        }

        /// <summary>
        /// 定位节点
        /// </summary>
        /// <param name="node">指定的节点</param>
        /// <returns>定位是否成功</returns>
        public bool SelectNode(XmlNode node)
        {
            if (null == node || null == this.m_xmlDoc || node.OwnerDocument != this.m_xmlDoc)
                return false;
            this.m_xmlSelectedNode = node as XmlElement;
            return this.m_xmlSelectedNode != null;
        }

        /// <summary>
        /// 定位节点
        /// </summary>
        /// <param name="nodePath">节点路径</param>
        /// <returns>定位是否成功</returns>
        public bool SelectNode(string nodePath)
        {
            if (null == this.m_xmlRootNode || String.IsNullOrWhiteSpace(nodePath))
                return false;
            List<string> nodeList = XmlHelper.SplitNodePath(nodePath);

            // 判断传入的定位路径中是否已经带有根节点，若没有则在最前面加上根节点
            if (nodeList.Count == 0 || nodeList[0] != this.m_xmlRootNode.Name)
            {
                nodeList.Insert(0, this.m_xmlRootNode.Name);
            }

            // 把分解后的路径重新整合
            string newNodePath = string.Empty;
            foreach (string nodeName in nodeList)
            {
                newNodePath += String.Format("/{0}", nodeName);
            }

            try
            {
                // 定位
                this.m_xmlSelectedNode = this.m_xmlRootNode.SelectSingleNode(newNodePath) as XmlElement;
                return this.m_xmlSelectedNode != null;
            }
            catch (System.Exception)
            {
                this.m_xmlSelectedNode = null;
                return false;
            }
        }

        /// <summary>
        /// 定位到根节点
        /// </summary>
        /// <returns>定位是否成功</returns>
        public bool SelectRootNode()
        {
            if (null == this.m_xmlDoc || null == this.m_xmlRootNode)
                return false;
            this.m_xmlSelectedNode = this.m_xmlRootNode;
            return true;
        }

        /// <summary>
        /// 判断当前节点是否有子节点，通常情况下先调用SelectNode进行定位
        /// </summary>
        public bool HasChildNodes
        {
            get
            {
                // 是否已经准备就绪
                if (!this.IsReady())
                    return false;
                return this.m_xmlSelectedNode.HasChildNodes;
            }
        }

        /// <summary>
        /// 获取当前节点下所有子节点元素，通常情况下先调用SelectNode进行定位
        /// </summary>
        /// <returns>子节点集</returns>
        public List<XmlElement> GetElements()
        {
            List<XmlElement> nodes = new List<XmlElement>();
            // 是否已经准备就绪
            if (!this.IsReady())
                return nodes;
            foreach (XmlNode node in this.m_xmlSelectedNode.ChildNodes)
            {
                nodes.Add(node as XmlElement);
            }

            return nodes;
        }

        /// <summary>
        /// 获取当前节点下所有属性，通常情况下先调用SelectNode进行定位
        /// </summary>
        /// <returns>属性集</returns>
        public List<XmlAttribute> GetAttributes()
        {
            List<XmlAttribute> attributes = new List<XmlAttribute>();
            // 是否已经准备就绪
            if (!this.IsReady())
                return attributes;
            foreach (XmlAttribute item in this.m_xmlSelectedNode.Attributes)
            {
                attributes.Add(item);
            }
            return attributes;
        }

        /// <summary>
        /// 获取当前节点的值，通常情况下先调用SelectNode进行定位
        /// </summary>
        /// <returns>节点的值</returns>
        public string GetValue()
        {
            // 是否已经准备就绪
            if (!this.IsReady())
                return string.Empty;
            return this.m_xmlSelectedNode.InnerText;
        }

        /// <summary>
        /// 获取当前节点的值，通常情况下先调用SelectNode进行定位
        /// </summary>
        /// <param name="defaultValue">默认返回值。若获取失败，就返回该默认值</param>
        /// <returns>节点的值</returns>
        public string GetValueEx(string defaultValue = "")
        {
            string value = this.GetValue();
            return String.IsNullOrWhiteSpace(value) ? defaultValue : value;
        }

        /// <summary>
        /// 获取当前节点的属性值，通常情况下先调用SelectNode进行定位
        /// </summary>
        /// <param name="name">属性名称</param>
        /// <returns>属性值</returns>
        public string GetAttributeValue(string name)
        {
            // 是否已经准备就绪
            if (!this.IsReady())
                return string.Empty;
            return this.m_xmlSelectedNode.GetAttribute(name);
        }

        /// <summary>
        /// 获取当前节点的属性值，通常情况下先调用SelectNode进行定位
        /// </summary>
        /// <param name="name">属性名称</param>
        /// <param name="defValue">默认返回值。若获取失败，就返回该默认值</param>
        /// <returns>属性值</returns>
        public string GetAttributeValueEx(string name, string defValue = "")
        {
            string value = this.GetAttributeValue(name);
            return string.IsNullOrWhiteSpace(value) ? defValue : value;
        }

        /// <summary>
        /// 添加子节点，通常情况下先调用SelectNode进行定位 
        /// </summary>
        /// <param name="nodeName">子节点名称</param>
        /// <returns>新添加的节点</returns>
        public XmlElement AddElement(string nodeName)
        {
            // 是否已经准备就绪
            if (!this.IsReady())
                return null;

            try
            {
                // 创建子节点并添加
                XmlElement xmlElement = m_xmlDoc.CreateElement(nodeName);
                this.m_xmlSelectedNode.AppendChild(xmlElement);
                return xmlElement;
            }
            catch (System.Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// 添加子节点，通常情况下先调用SelectNode进行定位 
        /// </summary>
        /// <param name="nodeName">子节点名称</param>
        /// <param name="value">子节点的值</param>
        /// <returns>新添加的节点</returns>
        public XmlElement AddElement(string nodeName, string value)
        {
            XmlElement xmlElement = this.AddElement(nodeName);
            if (null == xmlElement)
                return null;
            xmlElement.InnerText = value;
            return xmlElement;
        }

        /// <summary>
        /// 添加子节点，通常情况下先调用SelectNode进行定位 
        /// </summary>
        /// <param name="xmlElement">子节点</param>
        /// <returns>新添加的节点</returns>
        public XmlElement AddElement(XmlElement xmlElement)
        {
            if (null == xmlElement || !this.IsReady())
                return null;
            if (xmlElement.OwnerDocument != this.m_xmlDoc)
                return null;

            try
            {
                XmlElement newXmlElement = this.m_xmlSelectedNode.AppendChild(xmlElement) as XmlElement;
                return newXmlElement;
            }
            catch (System.Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// 添加子节点，通常情况下先调用SelectNode进行定位 
        /// </summary>
        /// <param name="xmlElement">子节点</param>
        /// <param name="value">子节点的值</param>
        /// <returns>新添加的节点</returns>
        public XmlElement AddElement(XmlElement xmlElement, string value)
        {
            XmlElement newXmlElement = this.AddElement(xmlElement);
            if (null == newXmlElement)
                return null;
            newXmlElement.InnerText = value;
            return newXmlElement;
        }

        /// <summary>
        /// 设置当前节点的值，通常情况下先调用SelectNode进行定位 
        /// </summary>
        /// <param name="value">节点的值</param>
        /// <returns>判断是否设置成功</returns>
        public bool SetValue(string value)
        {
            // 是否已经准备就绪
            if (!this.IsReady())
                return false;
            this.m_xmlSelectedNode.InnerText = value;
            return true;
        }

        /// <summary>
        /// 设置当前节点的属性值，通常情况下先调用SelectNode进行定位
        /// </summary>
        /// <param name="name">属性名</param>
        /// <param name="value">属性值</param>
        /// <returns>判断是否设置成功</returns>
        public bool SetAttribute(string name, string value)
        {
            // 是否已经准备就绪
            if (!this.IsReady())
                return false;
            // 设置属性的值
            this.m_xmlSelectedNode.SetAttribute(name, value);
            return true;
        }

        /// <summary>
        /// 清除当前节点下所有子节点，通常情况下先调用SelectNode进行定位
        /// </summary>
        public bool ClearChildNodes()
        {
            return XmlHelper.ClearChildNodes(this.m_xmlSelectedNode);
        }
#endregion


#region 静态方法
        /// <summary>
        /// 获取指定节点下所有子节点元素 
        /// </summary>
        /// <param name="node">指定节点</param>
        /// <returns>子节点集合</returns>
        public static List<XmlElement> GetElements(XmlNode node)
        {
            List<XmlElement> childNodes = new List<XmlElement>();
            if (null == node)
                return childNodes;
            foreach (XmlNode childNode in node.ChildNodes)
            {
                childNodes.Add(childNode as XmlElement);
            }

            return childNodes; 
        }

        /// <summary>
        /// 清除指定节点下所有子节点
        /// </summary>
        /// <param name="node">指定节点</param>
        /// <returns>true：完成清除</returns>
        public static bool ClearChildNodes(XmlNode node)
        {
            if (null == node)
                return false;
            if (!node.HasChildNodes)
                return true;

            XmlNode childNode = node.FirstChild;
            if (null == childNode)
                return false;
            XmlNode nextNode = null;
            try
            {
                do
                {
                    nextNode = childNode.NextSibling;
                    node.RemoveChild(childNode);
                }
                while ((childNode = nextNode) != null);
                return true;
            }
            catch (System.Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 添加子节点。若路径中间节点不存在也自动添加；若整个路径都存在则只进行选择
        /// </summary>
        /// <param name="xmlDoc">XmlDocument对象</param>
        /// <param name="nodePath">节点路径</param>
        /// <returns></returns>
        public static XmlElement AddElement(XmlDocument xmlDoc, string nodePath)
        {
            if (null == xmlDoc || String.IsNullOrWhiteSpace(nodePath))
                return null;
            // 根节点
            XmlElement xmlRootNode = xmlDoc.DocumentElement;
            if (null == xmlRootNode)
                return null;
            // 分解节点路径
            List<string> nodeList = XmlHelper.SplitNodePath(nodePath);
            // 判断传入的定位路径中是否已经带有根节点，若没有则在最前面加上根节点
            if (nodeList.Count == 0 || nodeList[0] != xmlRootNode.Name)
            {
                nodeList.Insert(0, xmlRootNode.Name);
            }

            try
            {
                string newNodePath = string.Empty;
                XmlElement selectedNode = null;
                foreach (string nodeName in nodeList)
                {
                    newNodePath += String.Format("/{0}", nodeName);
                    if (xmlRootNode.SelectSingleNode(newNodePath) == null)
                    {
                        if (null == selectedNode)
                        {
                            return null;
                        }
                        else
                        {
                            // 创建子节点并添加
                            XmlElement xmlElement = xmlDoc.CreateElement(nodeName);
                            selectedNode = selectedNode.AppendChild(xmlElement) as XmlElement;
                        }
                    }
                    else
                    {
                        selectedNode = xmlRootNode.SelectSingleNode(newNodePath) as XmlElement;
                    }
                }

                return selectedNode;
            }
            catch (Exception)
            {
                return null;
            }
        }
#endregion

#region 内部使用
        /// <summary>
        /// 分解节点路径
        /// </summary>
        /// <param name="nodePath">节点路径</param>
        /// <returns>分解后的路径列表</returns>
        private static List<string> SplitNodePath(string nodePath)
        {
            List<string> nodeList = new List<string>();

            // 分解节点路径
            string[] nodeNames = nodePath.Trim().Split(m_sp, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < nodeNames.Length; i++)
            {
                nodeList.Add(nodeNames[i]);
            }

            return nodeList;
        }
#endregion
    }
}
