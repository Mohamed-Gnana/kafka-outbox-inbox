using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Outbox.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Outbox.Infra.Persistence.Configurations
{
    public class InboxConfigurations : IEntityTypeConfiguration<InboxMessage>
    {
        public void Configure(EntityTypeBuilder<InboxMessage> builder)
        {
            builder.ToTable("Inbox", "Messages");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.ConsumerType)
                   .IsRequired()
                   .HasMaxLength(255);

            builder.Property(x => x.ReceivedOn)
                   .IsRequired();
        }
    }
}
