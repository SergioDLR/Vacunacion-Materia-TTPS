import { TableRow, TableCell } from "@mui/material";
import dateParser from "@/components/utils/dateParser";
import numberParser from "@/components/utils/numberParser";
const Distribucion = ({ distribucion }) => {
  return (
    <TableRow>
      <TableCell>{distribucion?.descripcionJurisdiccion}</TableCell>
      <TableCell align="right">{numberParser(distribucion?.idLote)}</TableCell>
      <TableCell align="right">{dateParser(distribucion?.fechaEntrega)}</TableCell>
      <TableCell align="right">{numberParser(distribucion?.cantidadVacunas)}</TableCell>
      <TableCell align="right">{numberParser(distribucion?.aplicadas)}</TableCell>
      <TableCell align="right">{numberParser(distribucion?.vencidas)}</TableCell>
    </TableRow>
  );
};

export default Distribucion;
