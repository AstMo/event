using PartyMaker.Common.ErrorProvider;
using PartyMaker.Common.Impl.ErrorProvider;
using System.Collections.Generic;

namespace PartyMaker.Application.Service.ErrorProvider
{
    public class WebAppErrorMessageProvider : IErrorMessageProvider
    {
        private readonly Dictionary<string, string> _messages;

        public WebAppErrorMessageProvider()
        {
            _messages = new Dictionary<string, string>
            {
                { WebAppErrors.UsernameOrPasswordNullOrEmpty, "Логин или пароль не может быть пустым" },
                { WebAppErrors.WrongCredentials, "Неверная комбинация логин-пароль" },
                { WebAppErrors.UsernameIsNullOrEmtpy, "Имя пользователя не может быть пустым"},
                { WebAppErrors.PasswordNotEqual, "Пароли не совпадают" },
                { WebAppErrors.PasswordIsNullOrEmtpy, "Пароль не может быть пустым"},
                { WebAppErrors.PasswordNotCorrectLength, "Пароль должен быть в пределх от 3 до 20"},
                { WebAppErrors.EmailCannotBeEmpty, "Email не может быть пустым" },
                { WebAppErrors.EmailNotCorrectLength, "Email должен быть в пределах 3 до 100" },
                { WebAppErrors.EmailNotCorrect, "Email не корректый" },
                { WebAppErrors.PhoneCannotBeMoreLength, "Длина телефона не может быть больше 15 символов" },
                { WebAppErrors.UserRoleNotExist, "Данной роли не существует"},
                { WebAppErrors.ImageNotFound, "Image with id not found in system" },
                { WebAppErrors.UsernameNotCorrectLength, "Имя пользователя должно быть в пределх от 3 до 100"},
                { WebAppErrors.UserAlreadyExists, "Пользователь с таким именем уже существует в системе" },
                { WebAppErrors.UserNotFound, "Пользователь с таким именем не найдет в системе" },
                { WebAppErrors.EntityNotFound, "Сущность не найдена" },
                { WebAppErrors.BirthdayGreatedNow, "День рождения не может быть раньше сегодня" },
                { WebAppErrors.NameCannotBeEmpty, "Имя не может быть пустым" },
                { WebAppErrors.NameMaximumLength, "Имя не можеm быть больше 200 символов" },
                { WebAppErrors.NoteCannotBeEmpty, "Комментарий не может быть пустым" },
                { WebAppErrors.NoteMaximumLength, "Комментарий не можеm быть больше 200 символов" },
                { WebAppErrors.PercentageMustBeFromZeroToHundred, "Проценты не могут быть меньше 0 и больше 100" },
                { WebAppErrors.PatientIdCannotBeEmpty, "Пациент должен выбран" },
                { WebAppErrors.MixImageIdCannotBeEmpty, "Изображение должно быть загружено" },
                { WebAppErrors.UserCannotBeApproved, "Ссылка для подтверждения не действительна" },


                { WebAppErrors.NameEvetnMustbeFrom1To50Symbols, "Имя события должно быть длинной от 1 до 50 символов" },
                { WebAppErrors.NameEventIsNullOrEmtpy, "Имя события не может быть пустым" },
                { WebAppErrors.DateEventCannotBeEarlierThenToday, "Дата события не может быть раньше сегодня" },
                { WebAppErrors.LatitudeNotCorrect, "Широта должна быть в пределах от -90 до 90" },
                { WebAppErrors.LongitudeNotCorrect, "Долгота должна быть в пределах от 0 до 180" },
                { WebAppErrors.LengthOfAddressMustBeLess, "Длина адреса должна быть меньше 255" },
                { WebAppErrors.TotalBudgetMustBeLess, "Полный бюджет должен быть меньше 99999999999" },
                { WebAppErrors.UserDoesntHaveRights, "Данное действие не доступно пользователю" },

                { WebAppErrors.NameTaskIsNullOrEmpty, "Имя задачи не может быть пустым" },
                { WebAppErrors.NameTaskMustBeFrom1To50Symbols, "Имя задачи должно быть длинной от 1 до 50 символов" },
                { WebAppErrors.DescriptionTaskMaximum, "Описание задачи должно быь короче 1000 символов" },
                { WebAppErrors.EventNotExist, "Событие не существует" },
            };
        }

        public string GetErrorMessage(string key)
        {
            if (!_messages.TryGetValue(key, out string message))
            {
                return key;
            }

            return message;
        }
    }
}
