import React from 'react';
import { connect } from 'react-redux';
import { Modal, Button } from 'react-bootstrap';
import { CloseModalAction } from '../../redux/actions/modalAction';

class CustomModal extends React.Component {

  _onHide(){
    this.props.closeModal();
    if(this.props.onClose) this.props.onClose();
  }
  
  render() {
    const Body = this.props.body;
    return (
      <Modal
        show={this.props.show}
        onHide={() => this._onHide()}
        dialogClassName="modal-90w"
        aria-labelledby="modal-title"
      >
        <Modal.Header closeButton>
          <Modal.Title id="modal-title">
            {this.props.title}
          </Modal.Title>
        </Modal.Header>
        <Modal.Body>
          {Body !== null ? <Body /> : null}
        </Modal.Body>
        {this.props.hasFooter ?
          (<Modal.Footer>
            <Button variant="secondary" onClick={this.props.toggle}>
              Close
            </Button>
            <Button variant="primary" onClick={this.props.submitInfo}>
              Save Changes
            </Button>
          </Modal.Footer>) : null}
      </Modal>
    );
  }
}

const mapStateToProps = (state, ownProps) => {
  return { ...ownProps, ...state.modalReducer };
}

const mapDispatchToProps = dispatch => ({
  closeModal: () => { dispatch(CloseModalAction()); }
});

export default connect(mapStateToProps, mapDispatchToProps)(CustomModal);
