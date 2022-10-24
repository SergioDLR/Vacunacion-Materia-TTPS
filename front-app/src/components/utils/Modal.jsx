import * as React from "react";
import Box from "@mui/material/Box";
import Button from "@mui/material/Button";
import Typography from "@mui/material/Typography";
import Modal from "@mui/material/Modal";
import CustomButton from "./CustomButtom";

const style = {
  position: "absolute",
  top: "50%",
  left: "50%",
  transform: "translate(-50%, -50%)",
  width: 400,
  bgcolor: "background.paper",
  border: "2px solid #000",
  boxShadow: 24,
  p: 4,
};

const CustomModal = ({
  displayButton = true,
  children,
  title = "Open Modal",
  cerrar = "Cancelar",
  open,
  setOpen,
  color = "info",
  textColor = "#2E7994",
}) => {
  const handleOpen = () => setOpen(true);
  const handleClose = (event, reason) => {
    if (reason !== "backdropClick") {
      setOpen(false);
    }
  };
  return (
    <>
      {displayButton && (
        <CustomButton variant={"outlined"} color={color} textColor={textColor} onClick={handleOpen}>
          {title}
        </CustomButton>
      )}

      <div>
        <Modal
          open={open}
          onClose={handleClose}
          aria-labelledby="modal-modal-title"
          aria-describedby="modal-modal-description"
        >
          <Box sx={style}>{children}</Box>
        </Modal>
      </div>
    </>
  );
};

export default CustomModal;
