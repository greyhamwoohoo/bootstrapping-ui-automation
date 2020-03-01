using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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

        public AttachableSeleniumSession ReadSessionState()
        {
            if (!AttachableSessionExists)
            {
                return new AttachableSeleniumSession()
                {
                    IsValid = false
                };
            }

            try
            {
                var content = System.IO.File.ReadAllText(Path);
                var result = JsonConvert.DeserializeObject<AttachableSeleniumSession>(content);

                // NOTE: The 'Response' object that was serialized will not be deserialized properly. Rather than set up an Adapter
                //       and do it properly (!) I am going to cheat by coercing it this way.
                var contentAsJObject = JsonConvert.DeserializeObject<JObject>(content);
                var responseValueAsString = JsonConvert.SerializeObject(contentAsJObject["Response"]["Value"]);
                var responseValue = JsonConvert.DeserializeObject<Dictionary<string, object>>(responseValueAsString);

                result.Response.Value = responseValue;
                result.IsValid = true;
                return result;
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
