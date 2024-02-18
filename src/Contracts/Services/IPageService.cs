using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackMeUp.Contracts.Services;

public interface IPageService
{
    Type GetPageType(string key);
}