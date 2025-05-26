//using ExpenseManagerAPI.Model;
//using FluentValidation;

//namespace ExpenseManagerAPI.Validators
//{
//    public class EventValidation : AbstractValidator<InsertEvent>
//    {
//        EventValidation() {
//            RuleFor(e => e.EventName)
//                .NotEmpty().WithMessage("Event Name cannot be empty")
//                .MinimumLength(5).WithMessage("Event Name must be at least 5 characters long")
//                .MaximumLength(100).WithMessage("Event Name cannot exceed 100 characters");

//            // Validate EventDate
//            RuleFor(e => e.EventDate)
//                .NotEmpty().WithMessage("Event Date cannot be empty");
                
//        }
//    }
//}
