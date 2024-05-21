using System.Net.Http.Json;

using PaySpace.Calculator.Web.Services.Abstractions;
using PaySpace.Calculator.Web.Services.Models;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PaySpace.Calculator.Web.Services
{
    public class CalculatorHttpService : ICalculatorHttpService
    {
        public async Task<List<PostalCode>> GetPostalCodesAsync()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var response = await client.GetAsync("api/posta1code");
                    if (!response.IsSuccessStatusCode)
                    {
                        throw new Exception($"Cannot fetch postal codes, status code: {response.StatusCode}");
                    }

                    return await response.Content.ReadFromJsonAsync<List<PostalCode>>() ?? [];
                }
            }
            catch (ArgumentNullException ane)
            {
                throw new Exception("Cannot fetch postal codes - null argument - should not happen", ane);
            }
            catch (HttpRequestException hre)
            {
                throw new Exception("Cannot fetch postal codes - http exception", hre);
            }
            catch (JsonException je)
            {
                throw new Exception("Cannot fetch postal codes - json exception", je);
            }
            catch (Exception e)
            {
                throw new Exception("Cannot fetch postal codes", e);
            }
        }

        public async Task<List<CalculatorHistory>> GetHistoryAsync()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var response = await client.GetAsync("api/history");
                    if (!response.IsSuccessStatusCode)
                    {
                        throw new Exception($"Cannot fetch history, status code: {response.StatusCode}");
                    }

                    return await response.Content.ReadFromJsonAsync<List<CalculatorHistory>>() ?? [];
                }
            }
            catch (ArgumentNullException ane)
            {
                throw new Exception("Cannot fetch history - null argument", ane);
            }
            catch (HttpRequestException hre)
            {
                throw new Exception("Cannot fetch history - http exception", hre);
            }
            catch (JsonException je)
            {
                throw new Exception("Cannot fetch history - json exception", je);
            }
            catch (Exception e)
            {
                throw new Exception("Cannot fetch history", e);
            }
        }

        public async Task<CalculateResult> CalculateTaxAsync(CalculateRequest calculationRequest)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var response = await client.GetAsync("api/history");
                    if (!response.IsSuccessStatusCode)
                    {
                        throw new Exception($"Cannot fetch history, status code: {response.StatusCode}");
                    }

                    return await response.Content.ReadFromJsonAsync<CalculateResult>();
                }
            }
            catch (ArgumentNullException ane)
            {
                throw new Exception("Cannot fetch history - null argument", ane);
            }
            catch (HttpRequestException hre)
            {
                throw new Exception("Cannot fetch history - http exception", hre);
            }
            catch (JsonException je)
            {
                throw new Exception("Cannot fetch history - json exception", je);
            }
            catch (Exception e)
            {
                throw new Exception("Cannot fetch history", e);
            }
        }

        private async Task<T> callHTTPClient(string api)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var response = await client.GetAsync(api);
                    if (!response.IsSuccessStatusCode)
                    {
                        throw new Exception($"Cannot fetch {api}, status code:{response.StatusCode}");
                    }
                    await response.Content.ReadAsStringAsync().ContinueWith((Task<string> x) =>
                    {
                        if (x.IsFaulted)
                            throw x.Exception;

                        re = JsonConvert.DeserializeObject<T>(x.Result);
                    });

                    return await response.Content.ReadFromJsonAsync<List<T>>() ?? [];
                }
            }
            catch (ArgumentNullException ane)
            {
                throw new Exception("Cannot fetch history - null argument", ane);
            }
            catch (HttpRequestException hre)
            {
                throw new Exception("Cannot fetch history - http exception", hre);
            }
            catch (JsonException je)
            {
                throw new Exception("Cannot fetch history - json exception", je);
            }
            catch (Exception e)
            {
                throw new Exception("Cannot fetch history", e);
            }
        }
    }
}