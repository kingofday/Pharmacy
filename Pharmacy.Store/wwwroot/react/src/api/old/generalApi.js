import strings from './../shared/constant';
import addr from './addreses';

export default class generalApi {
    static async getContactUsInfo() {
        try {
            const response = await fetch(`${addr.getContactUs}`, {
                method: 'GET',
                mode: 'cors',
                headers: {
                    'Content-Type': 'application/json; charset=utf-8;'
                }
            });
            const rep = await response.json();
            console.log(rep);
            if (!rep.IsSuccessful)
                return {
                    success: false,
                    message: rep.Message
                }
            else
                return {
                    success: true,
                    result: {
                        whatsappLink: rep.Result.WhatsappLink,
                        telegramLink: rep.Result.TelegramLink,
                        phoneNumbers: rep.Result.PhoneNumbers,
                        websiteName: rep.Result.WebsiteName,
                        websiteUrl: rep.Result.WebsiteUrl
                    }
                };
        } catch (error) {
            return ({ success: false, message: strings.connecttionFailed });
        }

    }

}