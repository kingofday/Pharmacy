const baseUrl = 'https://localhost:44328/';

const addr = {
    getSingleStore:`${baseUrl}Store/GetSingle`,
    getProducts:`${baseUrl}Product/Get`,
    getSingleProduct:`${baseUrl}Product/GetSingle`,
    postCompleteInfo:`${baseUrl}order/CompleteInfo`,
    getAddresses:`${baseUrl}Address/Get`,
    getDeliveryCost:`${baseUrl}Address/GetDeliveryCost`,
    postOrder:`${baseUrl}Order/Add`,
    postTempBasket:`${baseUrl}Order/AddTempBasket`,
    getContactUs:`${baseUrl}Home/ContactUs`,
    getFixedBasket:`${baseUrl}TempOrderDetail/Get`
} 
export default addr;