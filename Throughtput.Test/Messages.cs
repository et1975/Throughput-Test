using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Throughtput.Test
{
    class DTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Int { get; set; }
        public int? MaybeInt { get; set; }
        public double Double { get; set; }
        public double? MaybeDouble { get; set; }
        public DateTime Date { get; set; }
        public DateTime? MaybeDate { get; set; }
        public byte[] Bytes { get; set; }
    }

    class Message
    {
        public IEnumerable<Guid> Ids { get; set; }
        public IEnumerable<DTO> Data { get; set; }

        public static Message Small
        {
            get { return new Message {Ids = new[] {Guid.NewGuid()}}; }
        }

        public static Message Medium
        {
            get
            {
                return new Message
                {
                    Ids = Enumerable.Range(1, 10).Select(_ => Guid.NewGuid()),
                    Data = Enumerable.Range(1, 10).Select(_ => new DTO
                            {
                                Id = Guid.NewGuid(),
                                Bytes = new byte[10]
                            }),
                };
            }
        }

        public static Message Large
        {
            get
            {
                return new Message
                {
                    Ids = Enumerable.Range(1, 100).Select(_ => Guid.NewGuid()),
                    Data = Enumerable.Range(1, 100).Select(_ => new DTO
                    {
                        Id = Guid.NewGuid(),
                        Name = DateTime.Now.ToString(),
                        MaybeDate = DateTime.Now,
                        MaybeDouble = DateTime.Now.Ticks,
                        MaybeInt = DateTime.Now.Second,
                        Bytes = new byte[100]
                    }),
                };
            }
        }
    }
}
