// // Infrastructure/Services/VerificationBackgroundService.cs
// using CandidateManagement.Application.Interfaces;
// using Microsoft.Extensions.DependencyInjection;
// using Microsoft.Extensions.Hosting;
// using Microsoft.Extensions.Logging;

// namespace CandidateManagement.Infrastructure.Services
// {
//     public class VerificationBackgroundService : BackgroundService
//     {
//         private readonly IServiceProvider _serviceProvider;
//         private readonly TimeSpan _processingInterval = TimeSpan.FromSeconds(30);

//         public VerificationBackgroundService(
//             IServiceProvider serviceProvider)
//         {
//             _serviceProvider = serviceProvider;
//         }

//         protected override async Task ExecuteAsync(CancellationToken stoppingToken)
//         {
//             while (!stoppingToken.IsCancellationRequested)
//             {
//                 try
//                 {
//                     await ProcessPendingVerificationsAsync(stoppingToken);
//                     await Task.Delay(_processingInterval, stoppingToken);
//                 }
//                 catch (Exception ex) when (ex is not OperationCanceledException)
//                 {
//                     await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken); // Wait before retry
//                 }
//             }
//         }

//         private async Task ProcessPendingVerificationsAsync(CancellationToken cancellationToken)
//         {
//             using var scope = _serviceProvider.CreateScope();
//             var verificationRepository = scope.ServiceProvider.GetRequiredService<IVerificationRepository>();
//             var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

//             var pendingVerifications = await verificationRepository.GetIncompleteVerificationsAsync(cancellationToken);
            
//             if (!pendingVerifications.Any())
//                 return;

//             foreach (var verification in pendingVerifications)
//             {
//                 try
//                 {
//                     // Здесь будет логика поиска кандидатов и сотрудников
//                     // Пока просто помечаем как завершенные для примера
//                     verification.CompleteVerification();
//                     await unitOfWork.SaveChangesAsync(cancellationToken);
//                 }
//                 catch (Exception ex)
//                 {
//                     verification.MarkAsFailed(ex.Message);
//                     await unitOfWork.SaveChangesAsync(cancellationToken);
//                 }
//             }
//         }
//     }
// }