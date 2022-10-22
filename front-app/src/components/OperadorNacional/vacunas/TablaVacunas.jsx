import { TableContainer, Table, TableHead, TableRow, TableCell, TableBody, Paper } from "@mui/material";
import Vacuna from "./Vacuna";
const TablaVacunas = ({ vacunas }) => {
  return (
    <TableContainer component={Paper}>
      <Table sx={{ minWidth: 650 }} size="small">
        <TableHead>
          <TableRow>
            <TableCell>Tipo vacuna</TableCell>
            <TableCell align="right">Descripcion</TableCell>
            <TableCell align="right">Tipo pandemia</TableCell>
            <TableCell align="right">Cantidad de dosis</TableCell>
            <TableCell align="right">Reglas</TableCell>
          </TableRow>
        </TableHead>
        <TableBody>
          {vacunas.map((element, index) => (
            <Vacuna vacuna={element} key={index} />
          ))}
        </TableBody>
      </Table>
    </TableContainer>
  );
};

export default TablaVacunas;
