import { useDispatch } from 'react-redux';
import { jobApiCallActions, modalActions, loadingActions } from '../_store';
import { UpsertForm } from './index';

const Modal = () => {
  const dispatch = useDispatch();

  const afterSubmit = async () => {
    dispatch(loadingActions.startLoading());
    await dispatch(modalActions.closeModal());
    await dispatch(jobApiCallActions.getAll({ page: 1 }));
    dispatch(loadingActions.stopLoading());
  };

  return (
    <div className="modal fade show" tabIndex="-1" style={{ display: 'block' }}>
      <div className="modal-dialog modal-lg modal-dialog-centered">
        <div className="modal-content">
          <div className="modal-header">
            <h5 className="modal-title">Create an Api Notifier</h5>
            <button
              type="button"
              className="btn-close"
              data-bs-dismiss="modal"
              aria-label="Close"
              onClick={() => dispatch(modalActions.closeModal())}
            ></button>
          </div>
          <div className="modal-body">
            <UpsertForm id={null} afterSubmit={afterSubmit} />
          </div>
        </div>
      </div>
      <div className="modal-backdrop fade show"></div>
    </div>
  );
};
export { Modal };
