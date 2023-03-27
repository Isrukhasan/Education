using AutoMapper;
using Education.Core.BLL.Interfaces;
using Education.Core.Common.Dtos.Requests.Security;
using Education.Core.Common.Dtos.User;
using Education.Core.DALL.Intefaces.Repositories;

namespace Education.Core.BLL.Services
{
    public class SecurityService : ISecurityService
    {
        private readonly ISecurityRepository _securityRepository;
        private readonly IBCryptService _bCryptService;
        private readonly IMapper _mapper;
        public SecurityService(ISecurityRepository securityRepository,
            IBCryptService bCryptService, IMapper mapper
            )
        {
            _securityRepository = securityRepository;
            _bCryptService = bCryptService;
            _mapper = mapper;
        }

        /// <summary>
        /// This method gets web user auth data by credentials and verify its authentication
        /// </summary>
        /// <param name="request">Credentials sended to API</param>
        /// <returns>Web user auth data</returns>
        public async Task<WebUserAuthDataDto> GetWebUserAuthDataByCredentialsAsync(WebUserLoginRequestDto request)
        {
            var dbUserAuth = await _securityRepository.GetWebUserAuthDataByCredentialsAsync(request.UserName);

            if (dbUserAuth == null || !request.UserName.Equals(dbUserAuth.UserName) || !_bCryptService.VerifyPasswords(request.PassWord, dbUserAuth.PassWord))
            {
                return new WebUserAuthDataDto() { IsAuthenticated = false };
            }

            var userAuth = _mapper.Map<WebUserAuthDataDto>(dbUserAuth);

            userAuth.IsAuthenticated = true;

            return userAuth;
        }
    }
}
