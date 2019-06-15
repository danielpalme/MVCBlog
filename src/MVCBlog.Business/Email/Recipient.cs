using System;

namespace MVCBlog.Business.Email
{
    public class Recipient
    {
        public Recipient(string address)
        {
            this.Address = address ?? throw new ArgumentNullException(nameof(address));
        }

        public Recipient(string name, string address)
        {
            this.Name = name ?? throw new ArgumentNullException(nameof(name));
            this.Address = address ?? throw new ArgumentNullException(nameof(address));
        }

        public string Name { get; }

        public string Address { get; }

        public override string ToString()
        {
            return this.Address;
        }
    }
}
