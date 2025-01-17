﻿using System;
using System.Collections.Generic;
using System.Text;
using SAPbouiCOM;

namespace btnPrintOnForm
{
    class Events
    {
        private SAPbouiCOM.Application SBO_Application;
        private void setApplication()
        {
            SAPbouiCOM.SboGuiApi sboGuiApi = null;
            string sConnectionString = null;
            sboGuiApi = new SAPbouiCOM.SboGuiApi();

            sConnectionString = System.Convert.ToString(Environment.GetCommandLineArgs().GetValue(1));
            sboGuiApi.Connect(sConnectionString);

            SBO_Application = sboGuiApi.GetApplication();
        }

        public Events()
        {
            setApplication();

            SBO_Application.AppEvent += new SAPbouiCOM._IApplicationEvents_AppEventEventHandler(SBO_Application_AppEvent);
            SBO_Application.ItemEvent += new SAPbouiCOM._IApplicationEvents_ItemEventEventHandler(SBO_Application_ItemEvent);
        }
        


        private void SBO_Application_ItemEvent(string FormUID, ref SAPbouiCOM.ItemEvent pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
            if ((pVal.FormType == 60150 && pVal.EventType != SAPbouiCOM.BoEventTypes.et_FORM_UNLOAD) && pVal.BeforeAction == true)
                formAttrezzatura.EventsAttrezzatura.Events(ref pVal, ref SBO_Application);
            else if ((pVal.FormType == 60110 && pVal.EventType != SAPbouiCOM.BoEventTypes.et_FORM_UNLOAD) && pVal.BeforeAction == true)
                formChiamate.EventsChiamate.Events(ref pVal, ref SBO_Application);
            else if (pVal.FormType == 0 && pVal.BeforeAction == true)
                Events_FormBase(pVal, ref SBO_Application);
        }

        private void Events_FormBase(SAPbouiCOM.ItemEvent pVal, ref SAPbouiCOM.Application SBO_Application)
        {
            SAPbouiCOM.Form oForm = SBO_Application.Forms.GetForm(pVal.FormType.ToString(), pVal.FormTypeCount);
            if(pVal.ItemUID == "1" && oForm.Items.Item("1").Type.ToString() == "it_EDIT" )
                formStatusbar.form.click_Statusbar(pVal, ref SBO_Application);
            if(pVal.ItemUID == "2" && oForm.Items.Item("2").Type.ToString() == "it_BUTTON")
                formAttrezzatura.EventsAttrezzatura.click_btnSaveTxt(oForm, ref SBO_Application);
	    if(pVal.ItemUID == "3" && oForm.Items.Item("3").Type.ToString() == "it_BUTTON")
            formAttrezzatura.EventsAttrezzatura.click_btnSaveJson(oForm, ref SBO_Application);
	}
       
        private void SBO_Application_AppEvent(SAPbouiCOM.BoAppEventTypes EventType)
        {
            switch (EventType)
            {
                case SAPbouiCOM.BoAppEventTypes.aet_ShutDown:
                    //Exit Add-On
                    System.Windows.Forms.Application.Exit();
                    break;
                case SAPbouiCOM.BoAppEventTypes.aet_CompanyChanged:
                    break;
                case SAPbouiCOM.BoAppEventTypes.aet_FontChanged:
                    break;
                case SAPbouiCOM.BoAppEventTypes.aet_LanguageChanged:
                    break;
                case SAPbouiCOM.BoAppEventTypes.aet_ServerTerminition:
                    break;
                default:
                    break;
            }
        }
    }
}
