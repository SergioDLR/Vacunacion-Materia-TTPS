export const urlBase = "https://localhost:5001/api";

const allUrls = {
  user: `${urlBase}/Usuarios/`,
  jurisdiccion: `${urlBase}/Jurisdicciones/`,
  roles: `${urlBase}/Roles/`,
  vacunas: `${urlBase}/TiposVacunas/`,
  pandemias: `${urlBase}/Pandemias/`,
  calendario: `${urlBase}/Vacunas/GetDescripcionesVacunasCalendario`,
  crearVacuna: `${urlBase}/Vacunas/CrearVacuna`,
  descripcionAnual: `${urlBase}/Vacunas/GetDescripcionesVacunasAnuales`,
  todasVacunas: `${urlBase}/Vacunas/GetAll`,
};
export default allUrls;
