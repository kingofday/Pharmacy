import addr from './addresses';
import strings from './../shared/constant';

export default class apiDrug {

    static async signUp(model) {
        try {
            const response = await fetch(addr.signUp, {
                method: 'POST',
                mode: 'cors',
                headers: {
                    'Content-Type': 'application/json; charset=utf-8;'
                },
                body: JSON.stringify(model)
            });
            const rep = await response.json();
            if (!rep.IsSuccessful)
                return { success: false, message: rep.Message };
            console.log(rep.Result);
            let user = rep.Result;
            return {
                success: true,
                result: rep.Result.MobileNumber
            }
        } catch (error) {
            return ({ success: false, message: strings.connectionFailed });
        }
    }

    static async signIn(model) {
        try {
            const response = await fetch(addr.signIn, {
                method: 'POST',
                mode: 'cors',
                headers: {
                    'Content-Type': 'application/json; charset=utf-8;'
                },
                body:JSON.stringify(model)
            });
            const rep = await response.json();
            if (!rep.IsSuccessful)
                return { success: false, message: rep.Message };
            console.log(rep.Result);
            let user = rep.Result;
            return {
                success: true,
                result: {
                    email: user.Email,
                    mobileNumber: user.MobileNumber,
                    fullname: user.FullName,
                    token: user.Token,
                    isConfirmed: user.IsConfirmed
                }
            }
        } catch (error) {
            return ({ success: false, message: strings.connectionFailed });
        }
    }

    static async confirm(mobileNumber, code) {
        try {
            const response = await fetch(addr.confirm, {
                method: 'POST',
                mode: 'cors',
                headers: {
                    'Content-Type': 'application/json; charset=utf-8;'
                },
                body:JSON.stringify({
                    mobileNumber, 
                    code
                })
            });
            const rep = await response.json();
            if (!rep.IsSuccessful)
                return { success: false, message: rep.Message };
            let user = rep.Result;
            return {
                success: true,
                result: {
                    email: user.Email,
                    mobileNumber: user.MobileNumber,
                    fullname: user.Fullname,
                    token: user.Token,
                    isConfirmed: user.IsConfirmed
                }
            }
        } catch (error) {
            return ({ success: false, message: strings.connectionFailed });
        }
    }

    static async resendSMS(mobileNumber) {
        try {
            const response = await fetch(addr.resendSMS(mobileNumber), {
                method: 'POST',
                mode: 'cors',
                headers: {
                    'Content-Type': 'application/json; charset=utf-8;'
                }
            });
            const rep = await response.json();
            return {success: rep.IsSuccessful}

        } catch (error) {
            return ({ success: false, message: strings.connectionFailed });
        }
    }
}