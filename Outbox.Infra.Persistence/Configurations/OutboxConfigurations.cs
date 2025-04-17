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
    public class OutboxConfigurations : IEntityTypeConfiguration<OutboxMessage>
    {
        public void Configure(EntityTypeBuilder<OutboxMessage> builder)
        {
            builder.ToTable("Outbox", "Messages");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Type)
                   .IsRequired()
                   .HasMaxLength(255);

            builder.Property(x => x.Content)
                   .IsRequired();

            builder.Property(x => x.OccurredOn)
                   .IsRequired();
        }
    }
}
