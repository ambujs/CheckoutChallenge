﻿using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Checkout.PaymentGateway.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Serilog;

namespace Checkout.PaymentGateway.HttpClientServices
{
    public class AcquiringBankClient : ControllerBase, IAcquiringBankClient
    {
        private readonly HttpClient _httpClient;

        public AcquiringBankClient(HttpClient client)
        {
            _httpClient = client;
        }

        public async Task<IActionResult> ProcessPayment(Payment payment)
        {
            var url = $"api/payment";
            var json = JsonConvert.SerializeObject(payment);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(url, data);

            // TODO: In real world the Acquirer API will have some auth headers here

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                return Ok(JsonConvert.DeserializeObject<PaymentResponse>(content));
            }

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return NotFound();
            }

            throw new HttpRequestException($"Error calling acquiring-bank: {response.StatusCode} {response.ReasonPhrase}");
        }

        public async Task<IActionResult> GetPayment(string paymentId)
        {
            var url = $"api/payment/{paymentId}";

            // TODO: In real world the Acquirer API will have some auth headers here

            var response = await _httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                return Ok(JsonConvert.DeserializeObject<Payment>(content));
            }

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return NotFound();
            }

            throw new HttpRequestException($"Error calling acquiring-bank: {response.StatusCode} {response.ReasonPhrase}");
        }
    }
}
