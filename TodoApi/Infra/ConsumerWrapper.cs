using Confluent.Kafka;
using System;
using System.Collections.Generic;
using System.Threading;

namespace TodoApi
{
    public class ConsumerWrapper
    {
        private string _topicName;
        private ConsumerConfig _consumerConfig;
        private IConsumer<string, string> _consumer;
        private static readonly Random rand = new Random();
        public ConsumerWrapper(ConsumerConfig config, string topicName)
        {
            this._topicName = topicName;
            this._consumerConfig = config;
            this._consumerConfig.AutoOffsetReset = AutoOffsetReset.Earliest;
            this._consumer = new ConsumerBuilder<string, string>(this._consumerConfig).Build();
          //  this._consumer.Subscribe(topicName);
        }
        public List<string> testMessage()
        {
            //var consumeResult = this._consumer.Consume();

            //return consumeResult.Value;

            List<string> msgLst = new List<string>();

            try
            {

                //this._consumer.Assign(new TopicPartitionOffset(this._topicName, new Partition(0), Offset.Beginning));
                //this._consumer.Seek(new TopicPartitionOffset(this._topicName, new Partition(0), Offset.Beginning));

                TopicPartitionOffset tps = new TopicPartitionOffset(new TopicPartition(this._topicName, 0), Offset.Beginning);
                this._consumer.Assign(tps);
                
                while (true)
                {
                    var result = this._consumer.Consume();
                    if (result == null || result.IsPartitionEOF)
                    {
                        Console.WriteLine("No messages...");
                        break;
                    }
                    else
                    {
                        Console.WriteLine($"Offset: {result.Offset}");
                        msgLst.Add(result.Value);
                    }
                }
            }
            catch (OperationCanceledException)
            {
                this._consumer.Close();
            }
            return msgLst;
        }
        public string readMessage()
        {

            //var consumeResult = this._consumer.Consume();
            //return consumeResult.Value;
            CancellationToken cancellationToken = new CancellationToken();
            const int commitPeriod = 5;

            string consumeResultvalue = "";
            using (var consumer = new ConsumerBuilder<Ignore, string>(this._consumerConfig)
               // Note: All handlers are called on the main .Consume thread.
               .SetErrorHandler((_, e) => Console.WriteLine($"Error: {e.Reason}"))
               .SetStatisticsHandler((_, json) => Console.WriteLine($"Statistics: {json}"))
               .SetPartitionsAssignedHandler((c, partitions) =>
               {
                   Console.WriteLine($"Assigned partitions: [{string.Join(", ", partitions)}]");
                   // possibly manually specify start offsets or override the partition assignment provided by
                   // the consumer group by returning a list of topic/partition/offsets to assign to, e.g.:
                   // 
                   // return partitions.Select(tp => new TopicPartitionOffset(tp, externalOffsets[tp]));
               })
               .SetPartitionsRevokedHandler((c, partitions) =>
               {
                   Console.WriteLine($"Revoking assignment: [{string.Join(", ", partitions)}]");
               })
               .Build())
            {

                consumer.Subscribe(_topicName);
                try
                {
                    // while (true)
                    // {
                    try
                    {
                        var consumeResult = consumer.Consume(cancellationToken);

                        if (consumeResult.IsPartitionEOF)
                        {
                            Console.WriteLine(
                                $"Reached end of topic {consumeResult.Topic}, partition {consumeResult.Partition}, offset {consumeResult.Offset}.");

                            //continue;
                            return "";
                        }

                        Console.WriteLine($"Received message at {consumeResult.TopicPartitionOffset}: {consumeResult.Value}");
                        consumeResultvalue = consumeResult.Value;
                        if (consumeResult.Offset % commitPeriod == 0)
                        {
                            // The Commit method sends a "commit offsets" request to the Kafka
                            // cluster and synchronously waits for the response. This is very
                            // slow compared to the rate at which the consumer is capable of
                            // consuming messages. A high performance application will typically
                            // commit offsets relatively infrequently and be designed handle
                            // duplicate messages in the event of failure.
                            try
                            {
                                consumer.Commit(consumeResult);
                            }
                            catch (KafkaException e)
                            {
                                Console.WriteLine($"Commit error: {e.Error.Reason}");
                            }
                        }
                    }
                    catch (ConsumeException e)
                    {
                        Console.WriteLine($"Consume error: {e.Error.Reason}");
                    }
                    // }
                }
                catch (OperationCanceledException)
                {
                    Console.WriteLine("Closing consumer.");
                    consumer.Close();
                }
            }
            return consumeResultvalue;
        }




    }
}

