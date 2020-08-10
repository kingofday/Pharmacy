import actionTypes from './../actions/actionTypes';
import strings from './../../shared/constant';
import srvAuth from './../../service/srvAuth';

const getInitilState = () => {
    if (!localStorage) {
        alert(strings.browserIsOld);
        return {
            goToNextPage:false,
            authenticated: false,
            token: null,
            mobileNumber: '',
            email: '',
            fullname: '',
            nextPage: '/'
        };
    }


    let rep = srvAuth.getUserInfo();
    if (rep.success)
        return {
            goToNextPage:false,
            authenticated: true,
            token: rep.result.token,
            fullname: rep.result.fullname,
            mobileNumber: rep.result.mobileNumber,
            email: rep.result.email,
        };
    else
        return {
            goToNextPage:false,
            authenticated: false,
            token: null,
            fullname: '',
            mobileNumber: '',
            email: ''
        };

}
const authReducer = (state = getInitilState(), action) => {
    switch (action.type) {
        case actionTypes.LOG_IN:
            return {
                ...state,
                authenticated: true,
                token: action.token,
                fullname: action.payload.fullname,
                mobileNumber: action.payload.mobileNumber,
                email: action.payload.email,
            };
        case actionTypes.LOG_OUT:
            return {
                ...state,
                authenticated: false,
                token: null,
                fullname: '',
                mobileNumber: '',
                email: ''
            };
        case actionTypes.SET_AUTH_NEXT_PAGE:
            return {
                ...state,
                nextPage: action.payload.nextPage
            };
        case actionTypes.AUTH_GOTO_NEXT_PAGE:
            console.log(action.payload);
            return {
                ...state,
                goToNextPage: action.payload.goToNextPage
            };
        default:
            return state;
    }
};


export default authReducer;