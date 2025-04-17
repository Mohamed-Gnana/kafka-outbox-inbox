# Kafka Outbox-Inbox Integration with MassTransit

This repository contains a **Proof of Concept (POC)** for integrating **Apache Kafka** with **MassTransit** in .NET. The project is designed with a **modular** and **packaged** architecture to demonstrate a scalable and maintainable message-based communication system.

Key features include:
- **Outbox Pattern**: Ensures reliable messaging by persisting messages before publishing, reducing the risk of message duplication.
- **Inbox Pattern**: Prevents the re-processing of the same message, addressing potential message duplication.
- **Custom Kafka Publisher**: Implements the outbox and inbox patterns in a Kafka-based messaging system.

---

## Technical Overview

The project is structured into several packages, each responsible for a specific aspect of the message flow:

### 1. **`Infra.Broker` Package**
   - **Purpose**: Abstracts the message publishing functionality for clients.
   - **Details**: 
     - Contains an interface `IPublisher` which must be implemented by every client to publish messages. The implementation depends on the client-specific messaging system (e.g., Kafka).
     - Includes only **MassTransit** as a dependency for messaging.
     - This package serves as the base for clients to integrate with MassTransit.

### 2. **`Infra.Broker.Kafka` Package**
   - **Purpose**: Kafka client implementation of the `IPublisher` interface.
   - **Details**: 
     - Implements the `IPublisher` interface to allow publishing messages to Kafka.
     - Uses **MassTransit's `IBusControl`** to manage the Kafka bus flow (`StartAsync` / `StopAsync`).
     - Uses **`ITopicProducerProvider`** to get the appropriate Kafka producer generically (`<T>`).
     - Includes a **Kafka Configuration Model** to define connection and producer settings.
     - Provides extension methods for managing Kafka-related operations.

### 3. **`Outbox.Domain` Package**
   - **Purpose**: Holds the entities for Outbox and Inbox messages, along with necessary abstractions.
   - **Details**:
     - Contains entities for **Outbox** and **Inbox** messages.
     - Defines the **`IMessagePublisher`** interface, abstracting the Outbox pattern functionality, which adds message persistence and delivery guarantees.
     - Clients can extend the `IPublisher` interface with the Outbox functionality by implementing `IMessagePublisher`.

### 4. **`Outbox.Infra.Persistence` Package**
   - **Purpose**: Provides the persistence mechanism for Outbox and Inbox messages using **Entity Framework**.
   - **Details**:
     - Implements message persistence through **Entity Framework** to store Outbox messages in a database.
     - Contains the **`OutboxProcessor`** class, which is used by a **WorkerService** to fetch unprocessed messages from the Outbox table and publish them.
     - Provides an **InboxFilter** to check for duplicate messages in the **Inbox** table to ensure messages are not processed more than once.
     - Includes the implementation of the **`IMessagePublisher`** interface, extending the publisher functionality to support Outbox message publishing.

### 5. **`Infra.Logger` Package**
   - **Purpose**: Provides generic logging functionality.
   - **Details**:
     - Contains a **generic logging model** for logging application events and errors.
     - Utilizes **Serilog** for logging configuration and output.

---

## Future Improvements

1. **Retry Mechanism for Inbox Messages**: Add support for retrying failed Inbox messages. This will ensure message delivery even if there is a temporary issue in processing the messages.
2. **Automatic Logging Extensions**: Provide extension methods for logging common events (e.g., error handling, message publishing). This will eliminate the need to manually specify log data each time an error occurs, making the logging process more streamlined.

---

## Installation and Setup

### Prerequisites
- **.NET 5/6/7/8** or later
- **MassTransit** for message bus integration
- **Apache Kafka** for message publishing
- **Entity Framework** for persistence

### Installation Steps

1. **Clone the Repository**:
   ```bash
   git clone https://github.com/your-username/kafka-outbox-inbox.git
   cd kafka-outbox-inbox

