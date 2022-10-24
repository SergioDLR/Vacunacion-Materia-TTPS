import { TableRow, TableCell, SvgIcon, Button } from "@mui/material";
import DeleteIcon from "@mui/icons-material/Delete";
import { useState } from "react";
import CustomModal from "@/components/utils/Modal";
import CustomButton from "@/components/utils/CustomButtom";

const VacunaDesarrollada = ({ vacuna }) => {
  const [open, setOpen] = useState(false);

  return (
    <TableRow>
      <TableCell>{vacuna.descripcionVacuna}</TableCell>
      <TableCell align="right">{vacuna.descripcionMarcaComercial}</TableCell>
      <TableCell align="right">{vacuna.diasDemoraEntrega}</TableCell>
      <TableCell align="right">{vacuna.precioVacuna} $</TableCell>
      <TableCell align="right">
        <CustomModal
          title={
            <SvgIcon>
              <DeleteIcon />
            </SvgIcon>
          }
          textColor={"red"}
          color={"error"}
          open={open}
          setOpen={setOpen}
        >
          <form>
            <h4>¿Esta seguro que desea eliminar la vacuna?</h4>
            <CustomButton variant={"contained"} color={"error"} onClick={() => setOpen(false)}>
              Cancelar
            </CustomButton>
            <CustomButton color={"success"} variant={"contained"} onClick={() => console.log("eliminando....")}>
              Confirmar
            </CustomButton>
          </form>
        </CustomModal>
      </TableCell>
    </TableRow>
  );
};

export default VacunaDesarrollada;
