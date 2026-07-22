using Microsoft.Extensions.Configuration; // Import configuration management for user secrets
using Microsoft.SemanticKernel; // Import Semantic Kernel core library
using Microsoft.SemanticKernel.Connectors.OpenAI; // Import OpenAI connector for GitHub Models compatibility
using Microsoft.SemanticKernel.TextToImage; // Import text-to-image service interface
using Microsoft.SemanticKernel.TextToAudio; // Import text-to-audio service interface
using System.Net.Http; // Import HttpClient for custom SSL configuration

// Get GitHub PAT from user secrets
IConfigurationRoot config = new ConfigurationBuilder().AddUserSecrets<Program>().Build(); // Build configuration to access user secrets
var githubPat = config["github_PAT"] ?? throw new InvalidOperationException("Missing GitHub PAT in user secrets"); // Retrieve GitHub PAT or throw exception if missing

// Create HttpClient with SSL bypass for development
var httpClientHandler = new HttpClientHandler(); // Create HTTP client handler
httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true; // Bypass SSL certificate validation for development
var httpClient = new HttpClient(httpClientHandler); // Create HttpClient with custom handler

// Create kernel with GitHub Models using OpenAI connector
Kernel githubKernel = Kernel.CreateBuilder() // Start building a new Semantic Kernel instance
    .AddOpenAIChatCompletion( // Add OpenAI-compatible chat completion service
        modelId: "meta/Llama-4-Maverick-17B-128E-Instruct-FP8", // Specify the GitHub Models model ID to use
        apiKey: githubPat, // Pass user secret token as the API key for authentication
        endpoint: new Uri("https://models.github.ai/inference"), // Set custom endpoint to GitHub Models inference API
        httpClient: httpClient // Pass custom HttpClient with SSL bypass
    )
    .Build(); // Build and finalize the kernel configuration

//// Example usage
//var prompt = "What is AI? Explain in max 20 words"; // Define the user prompt to send to the model
//Console.WriteLine($"User prompt >>> {prompt}"); // Display the user prompt to console

//var result = await githubKernel.InvokePromptAsync(prompt); // Invoke the kernel with the prompt and await the response
//Console.WriteLine($"Assistant answer >>> {result}"); // Display the assistant's response to console




//#region-CHAT COMPLETION USING SEMANTIC KERNEL

//var options = new OpenAIPromptExecutionSettings // Create execution settings to control model behavior

//{

//    MaxTokens = 50, // Limit the response to maximum 50 tokens

//    Temperature = 0.9 // Set temperature to 0.9 for more creative/random responses

//};

//var prompt = "Write a short poem about AI"; // Define the prompt for generating a short poem about AI

//var result = await githubKernel.InvokePromptAsync(prompt, new KernelArguments(options)); // Invoke GitHub kernel with prompt and custom settings

//Console.WriteLine(result); // Display the generated poem to console

//#endregion





//#region-CHAT COMPLETION USING SEMANTIC KERNEL WITH STREAMING

//var prompt = "Write a large poem about AI"; // Define the prompt for generating a large poem about AI

//string fullMessage = string.Empty; // Initialize empty string to accumulate the full response

//await foreach (var chatUpdate in githubKernel.InvokePromptStreamingAsync<StreamingChatMessageContent>(prompt)) // Stream the response chunk by chunk from GitHub kernel

//{

//    if (chatUpdate.Content is { Length: > 0 }) // Check if the current chunk has content

//    {

//        fullMessage += chatUpdate.Content; // Append the chunk content to the full message
//        Console.Write(chatUpdate.Content); // Write each chunk to console as it arrives for real-time display

//    }

//}

//#endregion





// #region-GENERATING AUDIO FILES
// // Note: GitHub Models does not support text-to-audio through OpenAI-compatible endpoint
// // This section uses OpenAI TTS service as GitHub Models only provides chat completion

// using Microsoft.SemanticKernel; // Import Semantic Kernel core library

// using Microsoft.SemanticKernel.Connectors.OpenAI; // Import OpenAI connector for TTS service

// using Microsoft.SemanticKernel.TextToAudio; // Import text-to-audio service interface

// using Microsoft.SemanticKernel.TextToImage; // Import text-to-image service interface



// var openAIKey = Environment.GetEnvironmentVariable("SKCourseOpenAIKey"); // Get OpenAI API key from environment variable

// var azureRegion = Environment.GetEnvironmentVariable("SKCourseAzureRegion"); // Get Azure region from environment variable

// var azureKey = Environment.GetEnvironmentVariable("SKCourseAzureKey"); // Get Azure API key from environment variable

// Kernel openAIKernel = Kernel.CreateBuilder() // Start building a new Semantic Kernel instance for OpenAI

//     .AddOpenAIChatCompletion("gpt-4o-mini-2024-07-18", $"{openAIKey}") // Add OpenAI chat completion service

//     .AddOpenAITextToImage($"{openAIKey}") // Add OpenAI text-to-image service

//     .AddOpenAITextToAudio("tts-1", $"{openAIKey}") // Add OpenAI text-to-audio service with TTS model

//     .Build(); // Build and finalize the OpenAI kernel configuration

// Kernel azureKernel = Kernel.CreateBuilder() // Start building a new Semantic Kernel instance for Azure

//     .AddAzureOpenAIChatCompletion("gpt-4o-mini", // Add Azure OpenAI chat completion service

//         $"{azureRegion}", // Pass Azure region for endpoint

//         $"{azureKey}") // Pass Azure API key for authentication

//     .AddAzureOpenAITextToImage("dall-e-3", endpoint: $"{azureRegion}", apiKey: $"{azureKey}") // Add Azure OpenAI text-to-image service

//     .AddAzureOpenAITextToAudio("tts-hd", endpoint: $"{azureRegion}", apiKey: $"{azureKey}") // Add Azure OpenAI text-to-audio service

//     .Build(); // Build and finalize the Azure kernel configuration

// var textToAudioService = openAIKernel.GetRequiredService<ITextToAudioService>(); // Get the text-to-audio service from the kernel

// var content = "Hello, my name is Semantic Kernel. I am a powerful AI tool that can help you with your projects. I can generate text, images, and audio for you. How can I help you today?"; // Define the text content to convert to audio

// OpenAITextToAudioExecutionSettings executionSettings = new OpenAITextToAudioExecutionSettings // Create execution settings for audio generation

// {

//     Voice = "alloy", // Set the voice type for the audio generation

//     ResponseFormat = "mp3", // Set the output audio format to MP3

//     Speed = 1.0f // Set the speech speed to normal (1.0)

// };

// var audioContent = // Variable to store the generated audio content

//     await textToAudioService.GetAudioContentAsync(content, executionSettings); // Call the service to generate audio from text with specified settings

// var path = "D:\\tests"; // Define the directory path where audio file will be saved

// var audioFilePath = Path.Combine(path, "audio.mp3"); // Combine directory and filename to create full file path

// await File.WriteAllBytesAsync(audioFilePath, audioContent.Data!.Value.ToArray()); // Write the audio data bytes to the specified file asynchronously

// Console.WriteLine("Finished"); // Print completion message to console

// #endregion




// #region-Generating images
// // Note: GitHub Models does not support text-to-image through OpenAI-compatible endpoint
// // This section uses OpenAI DALL-E service as GitHub Models only provides chat completion

// using Microsoft.SemanticKernel; // Import Semantic Kernel core library

// using Microsoft.SemanticKernel.Connectors.OpenAI; // Import OpenAI connector for DALL-E service

// using Microsoft.SemanticKernel.TextToImage; // Import text-to-image service interface

// var openAIKey = Environment.GetEnvironmentVariable("SKCourseOpenAIKey"); // Get OpenAI API key from environment variable

// var azureRegion = Environment.GetEnvironmentVariable("SKCourseAzureRegion"); // Get Azure region from environment variable

// var azureKey = Environment.GetEnvironmentVariable("SKCourseAzureKey"); // Get Azure API key from environment variable

// Kernel openAIKernel = Kernel.CreateBuilder() // Start building a new Semantic Kernel instance for OpenAI

//     .AddOpenAIChatCompletion("gpt-4o-mini-2024-07-18", $"{openAIKey}") // Add OpenAI chat completion service

//     .AddOpenAITextToImage($"{openAIKey}", modelId: "dall-e-3") // Add OpenAI text-to-image service with DALL-E 3 model

//     .Build(); // Build and finalize the OpenAI kernel configuration

// Kernel azureKernel = Kernel.CreateBuilder() // Start building a new Semantic Kernel instance for Azure

//     .AddAzureOpenAIChatCompletion("gpt-4o-mini", // Add Azure OpenAI chat completion service

//         $"{azureRegion}", // Pass Azure region for endpoint

//         $"{azureKey}") // Pass Azure API key for authentication

//     .AddAzureOpenAITextToImage("dall-e-3", endpoint: $"{azureRegion}", apiKey: $"{azureKey}") // Add Azure OpenAI text-to-image service

//     .Build(); // Build and finalize the Azure kernel configuration

// var service = openAIKernel.GetRequiredService<ITextToImageService>(); // Get the text-to-image service from the kernel

// var imagePrompt = "A beautiful sunset over the mountains"; // Define the prompt for image generation

// var options = new OpenAITextToImageExecutionSettings // Create execution settings for image generation

// {

//     Quality = "high", // Set the image quality to high

//     Size = (1024, 1792), // Set the image dimensions (width, height)

//     Style = "vivid" // Set the image style to vivid

// };

// var generatedImages = // Variable to store the generated image content

//     await service.GetImageContentsAsync(imagePrompt, executionSettings: options); // Call the service to generate images from the prompt with specified settings

// Console.WriteLine(generatedImages[0].Uri!.ToString()); // Print the URL of the first generated image to console
// #endregion

