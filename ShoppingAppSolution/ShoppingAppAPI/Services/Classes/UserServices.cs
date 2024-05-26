using Microsoft.EntityFrameworkCore;
using ShoppingAppAPI.Exceptions;
using ShoppingAppAPI.Models;
using ShoppingAppAPI.Models.DTO_s;
using ShoppingAppAPI.Repositories.Classes;
using ShoppingAppAPI.Repositories.Interfaces;
using ShoppingAppAPI.Services.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace ShoppingAppAPI.Services.Classes
{
    public class UserServices : IUserServices
    {
 
        private readonly IUserRepository _userRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IRepository<int, Seller> _sellerRepository;
        private readonly ITokenServices _tokenService;

        public UserServices(IUserRepository userRepository,
            ICustomerRepository customerRepository,
            IRepository<int, Seller> sellerRepository,
            ITokenServices tokenService
            )
        {
            _customerRepository = customerRepository;
            _userRepository = userRepository;
            _tokenService = tokenService;
            _sellerRepository = sellerRepository;
        }

        private byte[] EncryptPassword(string password, byte[] passwordSalt)
        {
            HMACSHA512 hMACSHA = new HMACSHA512(passwordSalt);
            var encrypterPass = hMACSHA.ComputeHash(Encoding.UTF8.GetBytes(password));
            return encrypterPass;
        }

        private bool ComparePassword(byte[] encrypterPass, byte[] password)
        {
            for (int i = 0; i < encrypterPass.Length; i++)
            {
                if (encrypterPass[i] != password[i])
                {
                    return false;
                }
            }
            return true;
        }
        private LoginReturnDTO MapUserToLoginReturn(User user)
        {
            LoginReturnDTO returnDTO = new LoginReturnDTO();
            returnDTO.Id = user.UserID;
            returnDTO.Role = user.Role;
            returnDTO.Token = _tokenService.GenerateToken(user);
            return returnDTO;
        }

        public async Task<LoginReturnDTO> CustomerLogin(LoginDTO loginDTO)
        {
            try
            {
                var user = await _userRepository.GetCustomerDetailByEmail(loginDTO.Email);
                if (user == null)
                {
                    throw new UnauthorizedUserException();
                }
                var encryptedPassword = EncryptPassword(loginDTO.Password, user.Password_Hashkey);
                bool isPasswordSame = ComparePassword(encryptedPassword, user.Password);
                if (isPasswordSame)
                {
                    if (user.Customer == null)
                    {
                        throw new UnauthorizedUserException();
                    }
                    LoginReturnDTO loginReturnDTO = MapUserToLoginReturn(user);
                    if (loginReturnDTO == null)
                    {
                        throw new CannotLoginException("Not able to login at this moment!");
                    }
                    return loginReturnDTO;
                }
                throw new UnauthorizedUserException();
            }
            catch (Exception e)
            {
                throw new CannotLoginException(e.Message);
            }
        }

        public async Task<RegisterReturnDTO> CustomerRegister(RegisterDTO userRegisterDTO)
        {
            try
            {
                var existingCustomer = await _customerRepository.GetCustomerByEmail(userRegisterDTO.Email);
                if (existingCustomer != null)
                {
                    throw new UserAlreadyExistsException("Email already exists");
                }
                if (userRegisterDTO.Password != userRegisterDTO.ConfirmPassword)
                {
                    throw new PasswordMismatchException("Password and Confirm Password do not match");
                }
                UserRegisterRepositoryDTO userRegisterRepositoryDTO = MapUserRegisterRepositoryDTO(userRegisterDTO);
                var (customer, user) = await _userRegisterRepository.AddCustomerUserWithTransaction(userRegisterRepositoryDTO);

                if (customer == null || user == null)
                {
                    throw new UnableToRegisterException("Error while adding user");
                }
                RegisterReturnDTO registerReturnDTO = MapCustomerToRegisterReturn(user, customer);
                return registerReturnDTO;
            }
            catch (Exception e)
            {
                throw new UnableToRegisterException(e.Message);
            }
        }

        public Task<LoginReturnDTO> SellerLogin(LoginDTO userLoginDTO)
        {
            throw new NotImplementedException();
        }

        public Task<RegisterReturnDTO> SellerRegister(RegisterDTO userRegisterDTO)
        {
            throw new NotImplementedException();
        }
    }
}
