import React from 'react';
import { Route, Redirect } from 'react-router-dom';
import { connect } from 'react-redux';
import { SetNexPage } from './../../../redux/actions/authAction';

class AuthRoute extends React.Component {
  componentDidMount() {
    this.props.setAuthNextPage(this.props.path);
  }

  render() {
    let Children = this.props.component;
    return (
      <Route
        exact
        path={this.props.path}
        {...this.props.rest}
        render={({ location }) =>
          this.props.authenticated ? (
            <Children {...this.props} />
          ) : (
              <Redirect
                to={{
                  pathname: "/",
                  state: { from: location }
                }}
              />
            )
        }
      />
    );
  }

}

const mapStateToProps = (state, ownProps) => {
  return { ...ownProps, ...state.authReducer };
}

const mapDispatchToProps = dispatch => ({
  setAuthNextPage: (nextPage) => dispatch(SetNexPage(nextPage))
});

export default connect(mapStateToProps, mapDispatchToProps)(AuthRoute);