using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Twitter.Json
{
    public class JsonParser
    {
        private static JsonParser c_jParser = null;
        private static object c_jParserLock = typeof(JsonParser);
        private JsonToken m_jtCurToken;
        private JsonTokenizer m_jtTokenizer;

        private JsonParser()
        {
            m_jtTokenizer = JsonTokenizer.GetTokenizer();
        }

        public static JsonParser GetParser()
        {
            lock (c_jParserLock)
            {
                if (c_jParser == null)
                    c_jParser = new JsonParser();
            }

            return c_jParser;
        }

        private void Match(JsonToken.TokenType jtExpected)
        {
            if (jtExpected == m_jtCurToken.Type)
                m_jtCurToken = m_jtTokenizer.GetNextToken();
            else
                throw new ApplicationException("Expected " + jtExpected.ToString() + ", got " + m_jtCurToken.Type.ToString());
        }

        private JsonNode Node()
        {
            JsonNode jnFinal = new JsonNode();
            string sKey;
            object objValue = null;

            Match(JsonToken.TokenType.OpenCurly);

            while (m_jtCurToken.Type != JsonToken.TokenType.CloseCurly)
            {
                sKey = m_jtCurToken.Text;
                Match(JsonToken.TokenType.String);
                Match(JsonToken.TokenType.Colon);

                switch (m_jtCurToken.Type)
                {
                    case JsonToken.TokenType.String:
                        objValue = Value(); break;
                    case JsonToken.TokenType.OpenCurly:
                        objValue = Node(); break;
                    case JsonToken.TokenType.OpenBracket:
                        objValue = List(); break;
                }

                jnFinal.Add(sKey, objValue);

                //we've found a comma! continue parsing key/value pairs
                if (m_jtCurToken.Type == JsonToken.TokenType.Comma)
                    Match(JsonToken.TokenType.Comma);
            }

            Match(JsonToken.TokenType.CloseCurly);
            return jnFinal;
        }

        private List<JsonObject> List()
        {
            List<JsonObject> ljoFinal = new List<JsonObject>();

            Match(JsonToken.TokenType.OpenBracket);

            while (m_jtCurToken.Type != JsonToken.TokenType.CloseBracket)
            {
                switch (m_jtCurToken.Type)
                {
                    case JsonToken.TokenType.String:
                        ljoFinal.Add(new JsonObject(Value())); break;
                    case JsonToken.TokenType.OpenCurly:
                        ljoFinal.Add(new JsonObject(Node())); break;
                    case JsonToken.TokenType.OpenBracket:
                        ljoFinal.Add(new JsonObject(List())); break;
                }

                //we've found a comma! continue parsing list elements
                if (m_jtCurToken.Type == JsonToken.TokenType.Comma)
                    Match(JsonToken.TokenType.Comma);
            }

            Match(JsonToken.TokenType.CloseBracket);
            return ljoFinal;
        }

        private string Value()
        {
            string sValue = m_jtCurToken.Text;
            Match(JsonToken.TokenType.String);
            return sValue;
        }

        public JsonDocument ParseStream(StreamReader stmRead)
        {
            m_jtTokenizer.InitWithStream(stmRead);
            m_jtCurToken = m_jtTokenizer.GetNextToken();

            switch (m_jtCurToken.Type)
            {
                case JsonToken.TokenType.OpenCurly:
                    return new JsonDocument(Node());
                case JsonToken.TokenType.OpenBracket:
                    return new JsonDocument(List());
                default:
                    return null;
            }
        }

        public JsonDocument ParseFile(string sFile)
        {
             return ParseStream(new StreamReader(sFile));
        }

        public JsonDocument ParseString(string sParseString)
        {
            return ParseStream(new StreamReader(new MemoryStream(Encoding.UTF8.GetBytes(sParseString))));
        }
    }
}
