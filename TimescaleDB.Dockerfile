FROM ghcr.io/cloudnative-pg/postgresql:15

USER root

# Update the package list and install dependencies
RUN apt-get update && \
    apt-get install -y gnupg postgresql-common apt-transport-https lsb-release wget

## Configure PostgreSQL APT repository
#RUN /usr/share/postgresql-common/pgdg/apt.postgresql.org.sh

# Add TimescaleDB's APT repository
RUN echo "deb https://packagecloud.io/timescale/timescaledb/debian/ $(lsb_release -c -s) main" | tee /etc/apt/sources.list.d/timescaledb.list

# Add TimescaleDB's GPG key
RUN wget --quiet -O - https://packagecloud.io/timescale/timescaledb/gpgkey | gpg --dearmor -o /etc/apt/trusted.gpg.d/timescaledb.gpg

# Update package list again and install TimescaleDB
RUN apt-get update && \
    apt-get install -y timescaledb-2-postgresql-15 postgresql-client && \
    apt-get clean && \
    rm -rf /var/lib/apt/lists/*

# Add the TimescaleDB library to the PostgreSQL shared library path
RUN echo "shared_preload_libraries='timescaledb'" >> /usr/share/postgresql/postgresql.conf.sample

## Ensure the entrypoint script is executable
#RUN chmod +x /usr/local/bin/docker-entrypoint.sh


USER 26