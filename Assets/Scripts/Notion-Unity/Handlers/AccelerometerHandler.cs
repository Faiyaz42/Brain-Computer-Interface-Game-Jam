﻿using Newtonsoft.Json;
using System.Text;
using System;
using System.Linq;
using UnityEngine;

namespace Notion.Unity
{
    public class AccelerometerHandler : IMetricHandler
    {
        public Metrics Metric => Metrics.Accelerometer;
        public string Label => string.Empty;

        private readonly StringBuilder _builder;

        public Action<Accelerometer> OnAccelerometerUpdated { get; set; }

        public AccelerometerHandler()
        {
            _builder = new StringBuilder();
        }

        public void Handle(string metricData)
        {
            Accelerometer accelerometer = JsonConvert.DeserializeObject<Accelerometer>(metricData);
            OnAccelerometerUpdated?.Invoke(accelerometer);

            _builder.AppendLine("Handling Accelerometer")
                .Append("X: ").AppendLine(accelerometer.X.ToString())
                .Append("Y: ").AppendLine(accelerometer.Y.ToString())
                .Append("Z: ").AppendLine(accelerometer.Z.ToString());

            //Debug.Log(_builder.ToString());
            _builder.Clear();
        }
    }
}