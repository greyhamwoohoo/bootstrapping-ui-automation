﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OpenQA.Selenium.Remote;
using System;
using System.Collections.Generic;
using TheInternet.Common.SessionManagement.Contracts;

namespace TheInternet.Common.SessionManagement
{
    public class AttachableSeleniumSessionStorage : IAttachableSeleniumSessionStorage
    {
        public const string SESSION_FILENAME = ".selenium.session";
        private readonly string _basePath;

        public AttachableSeleniumSessionStorage(string basePath)
        {
            if (null == basePath) throw new System.ArgumentNullException(nameof(basePath));

            _basePath = basePath;
        }

        public bool AttachableSessionExists => System.IO.File.Exists(Path);

        public string Path => System.IO.Path.Combine(_basePath, SESSION_FILENAME);

        public AttachableSeleniumSession ReadSessionState(string browserName)
        {
            if (null == browserName) throw new System.ArgumentNullException(nameof(browserName));

            if (!AttachableSessionExists)
            {
                return new AttachableSeleniumSession()
                {
                    IsValid = false
                };
            }

            try
            {
                var persistedSessionContent = System.IO.File.ReadAllText(Path);
                var persistedSession = JsonConvert.DeserializeObject<AttachableSeleniumSession>(persistedSessionContent);
                var persistedSessionContentAsJObject = JsonConvert.DeserializeObject<JObject>(persistedSessionContent);

                var officialResponse = Response.FromJson(persistedSession.OfficialResponse);
                officialResponse.SessionId = persistedSession.Response.SessionId;
                
                var newSession = new AttachableSeleniumSession()
                {
                    BrowserName = browserName,
                    IsValid = true,
                    OfficialResponse = persistedSession.OfficialResponse,
                    Response = officialResponse,
                    RemoteServerUri = persistedSession.RemoteServerUri,
                    CommandRepositoryTypeName = persistedSession.CommandRepositoryTypeName
                };

                var officialResponseValueDictionary = officialResponse.Value as Dictionary<string, object>;
                newSession.Response.Value = officialResponseValueDictionary["Value"];

                return newSession;
            }
            catch
            {
                RemoveSessionState();

                return new AttachableSeleniumSession()
                {
                    IsValid = false
                };
            }
        }

        public void RemoveSessionState()
        {
            if(System.IO.File.Exists(Path))
            {
                System.IO.File.Delete(Path);
            }
        }

        public void WriteSessionState(AttachableSeleniumSession session)
        {
            if (null == session) throw new ArgumentNullException(nameof(session));

            RemoveSessionState();

            var content = JsonConvert.SerializeObject(session);
            System.IO.File.WriteAllText(Path, content);
        }
    }
}
