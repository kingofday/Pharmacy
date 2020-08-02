import actionTypes from './../actions/actionTypes';
import strings from './../../shared/constant';
import authSrv from './../../service/authSrv';

const getInitilState = () => {
    if (!localStorage) {
        alert(strings.browserIsOld);
        return {
            authenticated: false,
            token: null,
            userId: null,
            username: ''
        };
    }


    let rep = authSrv.getUserInfo();
    if (rep.success)
        return {
            authenticated: true,
            token: rep.result.token,
            userId: rep.result.userId,
            username: rep.result.username
        };
    else
        return {
            authenticated: false,
            token: null,
            userId: null,
            username: ''
        };

}
const authenticationReducer = (state = getInitilState(), action) => {
    switch (action.type) {
        case actionTypes.LOGIN:
            return {
                ...state,
                authenticated: true,
                token: action.token,
                userId: action.userId,
                username: action.username
            };
        case actionTypes.LOGOUT:
            return {
                ...state,
                authenticated: false,
                token: null,
                userId: null,
                username: ''
            };
        default:
            return { ...state };
    }
};


export default authenticationReducer;