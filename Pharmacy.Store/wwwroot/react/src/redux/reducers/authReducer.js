import actionTypes from './../actions/actionTypes';
import strings from './../../shared/constant';
import srvUser from './../../service/srvUser';

const getInitilState = () => {
    if (!localStorage) {
        alert(strings.browserIsOld);
        return {
            goToNextPage:false,
            nextPage: '/',
            authenticated: false,
            token: null,
            mobileNumber: '',
            email: '',
            fullname: '', 
        };
    }
    let rep = srvUser.getUserInfo();
    if (rep.success)
        return {
            goToNextPage:false,
            nextPage:'/',
            authenticated: true,
            token: rep.result.token,
            fullname: rep.result.fullname,
            mobileNumber: rep.result.mobileNumber,
            email: rep.result.email,
        };
    else
        return {
            goToNextPage:false,
            nextPage:'/',
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
            console.log(action.payload);
            return {
                ...state,
                authenticated: true,
                token: action.token,
                fullname: action.payload.fullname,
                mobileNumber: action.payload.mobileNumber,
                email: action.payload.email,
            };
        case actionTypes.LOG_OUT:
            srvUser.removeUserInfo();

            return {
                ...state,
                authenticated: false,
                goToNextPage:false,
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
            return {
                ...state,
                goToNextPage: action.payload.goToNextPage
            };
        default:
            state.goToNextPage = false;
            return state;
    }
};


export default authReducer;