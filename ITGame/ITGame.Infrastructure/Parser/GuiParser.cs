using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace ITGame.Infrastructure.Parser
{
    public class GuiParser
    {
        private string entity;
        private DataTable propertiesTable;
        public GuiParser(string entity, DataTable propertiesTable) 
        {
            this.entity = entity;
            this.propertiesTable = propertiesTable;
        }

        public List<GuiData> Parse() 
        {
            List<GuiData> retList = new List<GuiData>();

            foreach (DataRow row in propertiesTable.Rows)
            {
                Dictionary<string, string> tmpDict = new Dictionary<string, string>();
                for (int index = 0; index < propertiesTable.Columns.Count; index++)
                {
                    tmpDict.Add(propertiesTable.Columns[index].ColumnName, row[index].ToString());
                }

                GuiData tmpData = new GuiData(entity, tmpDict);
                retList.Add(tmpData);
            }

            return retList;
        }
    }
}
