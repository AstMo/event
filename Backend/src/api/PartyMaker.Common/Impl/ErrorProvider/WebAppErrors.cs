using FluentValidation.Internal;
using System.Globalization;

namespace PartyMaker.Common.Impl.ErrorProvider
{
    public static class WebAppErrors
    {
        public static readonly string UsernameOrPasswordNullOrEmpty = "auth.nullorempty";
        public static readonly string WrongCredentials = "auth.wrongcredentials";
        public static readonly string UsernameIsNullOrEmtpy = "auth.usernameisempty";
        public static readonly string PasswordIsNullOrEmtpy = "auth.passwordisempty";
        public static readonly string UserRoleNotExist = "auth.userrolenotexist";
        public static readonly string UsernameNotCorrectLength = "auth.usernamenotcorrectsize";
        public static readonly string PasswordNotCorrectLength = "auth.passwordnotcorrectsize";
        public static readonly string PasswordNotEqual = "auth.passwordnotequal";
        public static readonly string EmailCannotBeEmpty = "auth.emailcannotbeempty";
        public static readonly string EmailNotCorrectLength = "auth.emailnotcorrectlength";
        public static readonly string EmailNotCorrect = "auth.emailnot correct";
        public static readonly string PhoneCannotBeMoreLength = "auth.phonemaximumlength";
        public static readonly string UserAlreadyExists = "reg.userexist";
        public static readonly string UserNotFound = "user.notfound";
        public static readonly string ImageNotFound = "image.notfound";
        public static readonly string EntityNotFound = "entity.notfound";
        public static readonly string BirthdayGreatedNow = "patient.birthdaygreatednow";
        public static readonly string NameCannotBeEmpty = "patient.namecannotbeempty";
        public static readonly string NameMaximumLength = "patient.namemaximumlength";
        public static readonly string NoteCannotBeEmpty = "patient.notecannotbeempty";
        public static readonly string NoteMaximumLength = "patient.notemaximumlength";
        public static readonly string PercentageMustBeFromZeroToHundred = "workrecord.percentagemustbefromzerotohundred";
        public static readonly string PatientIdCannotBeEmpty = "workrecord.patientidcannotbtempty";
        public static readonly string MixImageIdCannotBeEmpty = "workrecord.miximagecannotbyempty";
        public static readonly string UserCannotBeApproved = "auth.linkcannotbeapproved";

        /* ---------------- Events ----------------------------*/
        public static readonly string NameEvetnMustbeFrom1To50Symbols = "event.nameeventmustbelength";
        public static readonly string NameEventIsNullOrEmtpy = "event.namecannotbeempty";
        public static readonly string DateEventCannotBeEarlierThenToday = "event.datecannotbeearlierthantoday";
        public static readonly string LatitudeNotCorrect = "event.latitudemustbecorrect";
        public static readonly string LongitudeNotCorrect = "event.logitudeemustbecorrect";
        public static readonly string LengthOfAddressMustBeLess = "event.lengthofaddressmustbeless";
        public static readonly string TotalBudgetMustBeLess = "event.totalbudgetmustbeless";
        public static readonly string UserDoesntHaveRights = "event.userdoesnthaveright";

        /* ------------------ Task-----------------------------*/
        public static readonly string NameTaskMustBeFrom1To50Symbols = "task.nametaskmustbelength";
        public static readonly string NameTaskIsNullOrEmpty = "task.namecannotbeempty";
        public static readonly string DescriptionTaskMaximum = "task.descriptionmustbelength";
        public static readonly string EventNotExist = "task.eventnotexist";
    }
}
