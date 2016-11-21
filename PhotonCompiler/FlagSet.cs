using System.Collections.Generic;

namespace PhotonCompiler
{
    class FlagSet
    {
        Dictionary<string, string> _options = new Dictionary<string, string>();
        List<string> _args = new List<string>();

        public List<string> Args
        {
            get { return _args; }
        }

        public string StringOption(string name)
        {
            string v;
            if (_options.TryGetValue(name, out v))
            {
                return v;
            }

            return string.Empty;
        }

        public int IntOption(string name)
        {
            int r;

            int.TryParse(StringOption(name), out r);

            return r;
        }

        public bool BoolOption(string name)
        {
            bool r;

            bool.TryParse(StringOption(name), out r);

            return r;
        }

        public FlagSet(string[] args, int startIndex)
        {
            for (int i = startIndex; i < args.Length; i++)
            {
                var s = args[i];

                if (s.StartsWith("--"))
                {
                    if (_args.Count > 0)
                        return;

                    var kvstr = s.Split('=');
                    if (kvstr.Length != 2)
                    {
                        return;
                    }

                    _options.Add(kvstr[0], kvstr[1]);
                }
                else if (s.StartsWith("-"))
                {
                    if (_args.Count > 0)
                        return;

                    if (s.Length > 1)
                    {
                        _options.Add(s, "true");
                    }
                }
                else
                {
                    _args.Add(s);
                }
            }
        }
    }

}
